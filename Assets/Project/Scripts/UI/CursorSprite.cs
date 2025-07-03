using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorSprite : MonoBehaviour
{

    [SerializeField] private Image cursorImage;
    private List<Sprite> spriteStack = new List<Sprite>();
    private static CursorSprite instance;
    public static CursorSprite Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        UpdateSprite();
    }
    void Update()
    {
        if(spriteStack.Count != 0)
            transform.position = Input.mousePosition;
    }

    public void AddSprite(Sprite sprite, int index)
    {
        spriteStack.Insert(index, sprite);
        UpdateSprite();
    }
    public void AddSprite(Sprite sprite)
    {
        spriteStack.Add(sprite);
        UpdateSprite();
    }
    public void RemoveSprite(Sprite sprite)
    {
        if (sprite == null || !spriteStack.Contains(sprite))
        {
            return;
        }
        spriteStack.Remove(sprite);
        UpdateSprite();
    }
    public void SetCursorVisibility(bool visibility)
    {
        Cursor.visible = visibility;

        if (visibility) UpdateSprite();
        else cursorImage.gameObject.SetActive(false);
    }
    private void UpdateSprite()
    {
        if (spriteStack.Count == 0)
        {
            cursorImage.gameObject.SetActive(false);
            Cursor.visible = true;
        }
        else
        {
            transform.position = Input.mousePosition;
            cursorImage.gameObject.SetActive(true);
            cursorImage.sprite = spriteStack[0];
            Cursor.visible = false;
        }
    }
}
