using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pat : MonoBehaviour
{
    [SerializeField] private float loveSpeed = 5f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private ParticleSystem loveParticles;
    [SerializeField] private Sprite patCursorSprite;
    [SerializeField] private AudioClip patSound;
    [SerializeField] private float patSoundDelay = 0.25f;
    private bool isOpened = false;
    public bool IsOpened { get { return isOpened; } }
    private bool isPatStarted = false;
    private static Pat instance;
    public static Pat Instance { get { return instance; } }
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isOpened && isPatStarted && Input.GetMouseButtonUp(0))
        {
            Close();
        }
        if (isOpened && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, radius);

            bool isThereThePet = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<Pet>() != null)
                {
                    isThereThePet = true;
                }
            }
            if (isThereThePet) StartPat();
        }
        if (isPatStarted)
        {
            Pet.Instance.Stats.Love += loveSpeed * Time.deltaTime;
        }
    }
    public void Open()
    {
        isOpened = true;
        CursorSprite.Instance.AddSprite(patCursorSprite, 0);
        PetMenu.instance.Close();
    }
    public void Close()
    {
        isOpened = false;
        isPatStarted = false;
        Pet.Instance.RemoveIdleRequest("Pat");
        CursorSprite.Instance.SetCursorVisibility(true);
        Pet.Instance.Animator.SetBool("Pat", false);
        loveParticles.Stop();
    }
    public void StartPat()
    {
        ObjectInteraction.instance.DraggedObject = null;
        isPatStarted = true;
        Pet.Instance.AddIdleRequest("Pat");
        CursorSprite.Instance.RemoveSprite(patCursorSprite);
        CursorSprite.Instance.SetCursorVisibility(false);
        Pet.Instance.Animator.SetBool("Pat", true);
        loveParticles.Play();
        StartCoroutine(PatSoundPlaying());

        if (Random.Range(0, 10) <= 2)
        {
            MonologueWindow mw = MonologueWindow.instance;
            mw.RandomMonologueFromList(mw.PatMonologues);
        }
    }
    IEnumerator PatSoundPlaying()
    {
        while (isPatStarted)
        {
            yield return new WaitForSeconds(patSoundDelay);
            SoundManager.instance.PlaySound(patSound, 0.1f, SoundType.Main);
        }
    }
}
