using UnityEngine;

public class Pencil_PixelArtTool : PixelArtTool
{
    private Vector2Int? previousPosition = null;
    private const float LINE_THRESHOLD = 1.5f; // Максимальное расстояние для интерполяции
    public override bool IsToolUsed => draw;
    private bool draw = false;

    public override void OnHover(PixelArtCanvas canvas, Color activeColor, int x, int y)
    {
        Vector2Int currentPos = new Vector2Int(x, y);

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            draw = false;
            ApplyPreview(canvas);
        }
        // Сброс предыдущей позиции при отпускании кнопки
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            previousPosition = null;
            return;
        }

        // Обработка рисования
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (previousPosition == null)
            {
                draw = true;
                // Первый пиксель
                SetPixel(canvas, activeColor, currentPos);
                previousPosition = currentPos;
            }
            else if (previousPosition != currentPos)
            {
                // Рисуем линию между предыдущей и текущей позицией
                DrawLine(canvas, activeColor, previousPosition.Value, currentPos);
                previousPosition = currentPos;
            }
        }
    }

    private void SetPixel(PixelArtCanvas canvas, Color activeColor, Vector2Int pos)
    {
        
        Color color = Input.GetMouseButton(0) ? activeColor : Color.clear;
        if (canvas.GetPixel(pos.x, pos.y) == color) return;

        canvas.PreviewPixels[pos] = color;
        InvokePreviewChanged();
    }

    private void DrawLine(PixelArtCanvas canvas, Color activeColor, Vector2Int start, Vector2Int end)
    {
        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);
        int sx = start.x < end.x ? 1 : -1;
        int sy = start.y < end.y ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            SetPixel(canvas, activeColor, start);
            
            if (start.x == end.x && start.y == end.y) break;
            
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                start.x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                start.y += sy;
            }
        }
    }
}