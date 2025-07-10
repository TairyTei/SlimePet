using System.Collections;
using UnityEngine;

public class MinigameHideAndSeek : Task
{
    private int countOfCatches = 0;
    private float hideTime;
    private float timeToFind;
    private bool hidden = false;
    private MinigameManager minigameManager;
    public override bool IsDragAllowed { get { return false; } }
    public MinigameHideAndSeek(string taskName, float timeLimit, MinigameManager minigameManager) : base(taskName, timeLimit)
    {
        this.minigameManager = minigameManager;
    }

    IEnumerator Hide()
    {
        hidden = false;
        Pet.Instance.Animator.SetTrigger("JumpIn");
        yield return new WaitForSeconds(1f);
        Pet.Instance.transform.position = WindowBorderCollider.instance.GetRandomPointInRoom();
        Pet.Instance.Animator.SetTrigger("JumpOut");
        yield return new WaitForSeconds(0.2f);  
        hideTime = Time.time;
        timeToFind = 10 * Mathf.Pow(0.8f, countOfCatches);
        timeToFind = Mathf.Clamp(timeToFind, 0.5f, Mathf.Infinity);
        Debug.Log(timeToFind);
        hidden = true;
    }
    private void Catch()
    {
        countOfCatches++;
        minigameManager.SetValue_Score(countOfCatches);
        Pet.Instance.StartCoroutine(Hide());
    }
    public override void TaskStart()
    {
        Pet.Instance.StartCoroutine(Hide());
        minigameManager.SetActive_HideAndSeekUI(true);
        minigameManager.SetActive_MinigameUI(true);

        int record = PlayerPrefs.GetInt("HideAndSeek_Record");
        minigameManager.SetValue_Record(record);
        minigameManager.SetValue_Score(0);
    }
    public override void Actions()
    {
        if (Input.GetMouseButtonDown(0) && hidden)
        {
            Collider2D collider = ObjectInteraction.instance.ColliderUnderCursor;
            if (collider != null && collider.GetComponent<Pet>())
            {
                Catch();
            }
        }
        if (hidden)
        {
            minigameManager.SetValue_TimeSlider(1 - (Time.time - hideTime) / timeToFind);
        }
    }
    public override void TaskInterrupt() {}
    public override void TaskEnd()
    {
        Pet.Instance.Stats.Coins += countOfCatches;
        Pet.Instance.Stats.Expirience += countOfCatches * 2;
        minigameManager.SetActive_HideAndSeekUI(false);
        minigameManager.SetActive_MinigameUI(false);

        if (countOfCatches > PlayerPrefs.GetInt("HideAndSeek_Record"))
        {
            PlayerPrefs.SetInt("HideAndSeek_Record", countOfCatches);
            SoundManager.instance.PlaySound(minigameManager.RecordSound, 0, SoundType.System);
        }
        else
        {
            SoundManager.instance.PlaySound(minigameManager.MissSound, 0.3f, SoundType.Main);
        }
    }
    public override void TaskFail()
    {

    }

    public override bool isDone()
    {
        if (hidden && Time.time > hideTime + timeToFind) return true;
        return false;
    }
    public override bool isFailed()
    {
        return false;
    }
}
