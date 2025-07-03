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

    private bool isOpened = false;
    public bool IsOpened { get { return isOpened; } }

    private static Shower instance;
    public static Shower Instance {get{ return instance; }}
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (isOpened && Input.GetMouseButton(0))
        {
            Wash();
        }
        else
        {
            if (waterParticles.isPlaying)
            {
                waterParticles.Stop();
            }
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
    }
    public void Close()
    {
        isOpened = false;
        //TransparentWindow.Instance.ClickThrough = true;
        CursorSprite.Instance.RemoveSprite(showerCursorSprite);
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
    void OnDrawGizmos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.DrawWireSphere(mousePos + offset, radius);
    }
}
