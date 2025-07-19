using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    private Color color;
    private PixelArtEditor pixelArtEditor;
    [SerializeField] private Image image;

    public void Initialize(PixelArtEditor pixelArtEditor, Color color)
    {
        this.pixelArtEditor = pixelArtEditor;
        this.color = color;
        image.color = color;
    }
    public void Pick()
    {
        pixelArtEditor.SetColor(color);
    }
}