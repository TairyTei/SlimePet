using System.Collections;
using UnityEngine;

public class Poop : RoomObject, IObjectFunction
{
    public float maxHp = 3;
    public float regenerationDelay = 0.25f;
    [SerializeField]float hp = 3;
    private IEnumerator regenerationCoroutine;
    public AudioClip cleanSound;
    public GameObject coinPrefab;
    public override bool IsFunctional {get{ return true; }}

    void Start()
    {
        hp = maxHp;
    }
    public void RMB_Click()
    {
        if(regenerationCoroutine != null)
            StopCoroutine(regenerationCoroutine);
        regenerationCoroutine = Regeneration();
        StartCoroutine(regenerationCoroutine);
        hp -= 1f;
        Rescale();
        SoundManager.instance.PlaySound(cleanSound, 0.3f, SoundType.Main);
        if (hp <= 0)
        {
            if (UnityEngine.Random.Range(0, 4) == 1)
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            if (regenerationCoroutine != null)
                StopCoroutine(regenerationCoroutine);

            FurnitureBox.instance.RemoveFurnitureFromRoom(this);
            Destroy(gameObject);
        }
    }
    private void Rescale()
    {
       float scale = Mathf.Clamp(hp / maxHp, 0.5f, 1f);
        transform.localScale = new Vector2(scale, scale); 
    }
    public IEnumerator Regeneration()
    {
        yield return new WaitForSeconds(regenerationDelay);
        hp += 0.25f;
        Rescale();
        if (hp < maxHp)
        {
            if(regenerationCoroutine != null)
            StopCoroutine(regenerationCoroutine);
            regenerationCoroutine = Regeneration();
            StartCoroutine(regenerationCoroutine);
        }
        else
        {
            hp = Mathf.Clamp(hp, 0, maxHp);
        }
    }
}