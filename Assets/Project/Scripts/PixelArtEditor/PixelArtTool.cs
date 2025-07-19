using System;
using System.Collections.Generic;
using UnityEngine;

public class PixelArtTool
{
    public event Action OnChanged;
    public event Action OnPreviewChanged;

    public virtual void OnHover(PixelArtCanvas canvas, Color activeColor, int x, int y) { }
    public virtual void Cancel() { }
    protected virtual void ApplyPreview(PixelArtCanvas canvas)
    {
        PixelChangeCommand command = new PixelChangeCommand();
        foreach (Vector2Int pixelPos in canvas.PreviewPixels.Keys)
        {
            command.AddChange(pixelPos.x, pixelPos.y,canvas.GetPixel(pixelPos.x, pixelPos.y), canvas.PreviewPixels[pixelPos]);
        }
        canvas.CommandManager.ExecuteCommand(command, canvas);
        ClearPreview(canvas);
        InvokePreviewChanged();
        InvokeOnChanged();
    }
    protected void InvokeOnChanged() => OnChanged?.Invoke();
    protected void InvokePreviewChanged() => OnPreviewChanged?.Invoke();
    public virtual bool IsToolUsed => false;
    public void ClearPreview(PixelArtCanvas canvas) => canvas.PreviewPixels.Clear();
}