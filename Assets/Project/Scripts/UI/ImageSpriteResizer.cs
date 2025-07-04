using UnityEngine;
using UnityEngine.UI;

public class ImageSpriteResizer
{
    public static void Resize(Image image)
    {
        Sprite sprite = image.sprite;
        Vector2 spritePivot = new Vector2(
            sprite.pivot.x / sprite.rect.width,
            sprite.pivot.y / sprite.rect.height
        );
        image.rectTransform.pivot = spritePivot;
        image.SetNativeSize();
    }
}