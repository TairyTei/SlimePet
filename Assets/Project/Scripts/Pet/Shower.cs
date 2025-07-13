using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    [SerializeField] private float washSpeed = 5f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 particlesOffset;
    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private Sprite showerCursorSprite;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float soundDelay = 0.1f;

    private bool isOpened = false;
    private bool isWashing = false;
    public bool IsOpened { get { return isOpened; } }

    private static Shower instance;
    public static Shower Instance {get{ return instance; }}
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (isOpened && Input.GetMouseButton(0) && !isWashing)
        {
            isWashing = true;
            StartCoroutine(SoundPlaying());
        }
        else if (isOpened && !Input.GetMouseButton(0) && isWashing)
        {
            isWashing = false;

            waterParticles.Stop();

        }
        if (isWashing)
        {
            Wash();
        }
        if (isOpened && Input.GetMouseButtonDown(1))
        {
            Close();
        }
    }
    public void Open()
    {
        isOpened = true;
        //TransparentWindow.Instance.ClickThrough = false;
        CursorSprite.Instance.AddSprite(showerCursorSprite, 0);
        PetMenu.instance.Close();

        if (Random.Range(0, 10) <= 2)
        {
            MonologueWindow mw = MonologueWindow.instance;
            mw.RandomMonologueFromList(mw.CleanMonologues);
        }
    }
    IEnumerator SoundPlaying()
    {
        while (isWashing)
        {
            yield return new WaitForSeconds(soundDelay);
            SoundManager.instance.PlaySound(sound, 0.4f, SoundType.Main);
        }
    }
    public void Close()
    {
        isOpened = false;
        //TransparentWindow.Instance.ClickThrough = true;
        CursorSprite.Instance.RemoveSprite(showerCursorSprite);
        ObjectInteraction.instance.DraggedObject = null;
    }
    public void Wash()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos + offset, radius);

        bool isThereThePet = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Pet>() != null)
            {
                isThereThePet = true;
            }
        }
        if (isThereThePet)
        {
            Pet.Instance.Stats.Cleanliness += washSpeed * Time.deltaTime;
        }
        waterParticles.transform.position = mousePos + particlesOffset;
        if (!waterParticles.isPlaying)
        {
            waterParticles.Play();
        }
    }
  
}
