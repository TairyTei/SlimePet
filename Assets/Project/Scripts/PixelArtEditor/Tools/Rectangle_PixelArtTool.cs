using UnityEngine;

public class Rectangle_PixelArtTool : PixelArtTool
{
    private Vector2Int startPos;
    private bool isDrawing = false;
    public override void OnHover(PixelArtCanvas canvas, Color activeColor, int x, int y)
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = new Vector2Int(x, y);
            isDrawing = true;
        }

        if (isDrawing && Input.GetMouseButtonUp(0))
        {
            x = Mathf.Clamp(x, 0, canvas.canvasSize.x - 1);
            y = Mathf.Clamp(y, 0, canvas.canvasSize.y - 1);


            int minX = Mathf.Min(startPos.x, x);
            int maxX = Mathf.Max(startPos.x, x);
            int minY = Mathf.Min(startPos.y, y);
            int maxY = Mathf.Max(startPos.y, y);

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {

                    canvas.SetPixel(i, j, activeColor);

                }
            }

            InvokeOnChanged();
            isDrawing = false;
        }
    }
}