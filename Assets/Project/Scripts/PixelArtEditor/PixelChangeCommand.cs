using System.Collections.Generic;
using UnityEngine;

public class PixelChangeCommand : CanvasCommand
{
    private struct PixelChange
    {
        public Vector2Int position;
        public Color originalColor;
        public Color newColor;
    }

    private List<PixelChange> changes = new List<PixelChange>();

    public void AddChange(int x, int y, Color originalColor, Color newColor)
    {
        changes.Add(new PixelChange {
            position = new Vector2Int(x, y),
            originalColor = originalColor,
            newColor = newColor
        });
    }

    public override void Execute(PixelArtCanvas canvas)
    {
        foreach (var change in changes)
        {
            canvas.SetPixel(change.position.x, change.position.y, change.newColor);
        }
    }

    public override void Undo(PixelArtCanvas canvas)
    {
        foreach (var change in changes)
        {
            canvas.SetPixel(change.position.x, change.position.y, change.originalColor);
        }
    }
}