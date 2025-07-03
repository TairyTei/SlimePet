// DynamicSpriteSwapper.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSpriteSwapper : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void LateUpdate()
    {
        if (_spriteRenderer.sprite != null)
        {
            string spriteName = _spriteRenderer.sprite.name;
            
            Sprite newSprite = SkinManger.instance.GetSprite(spriteName);
            
            if (newSprite != null)
            {
                _spriteRenderer.sprite = newSprite;
            }
        }
    }
}