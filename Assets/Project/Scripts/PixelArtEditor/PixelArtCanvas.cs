using System.Collections.Generic;
using UnityEngine;

public class PixelArtCanvas
{
    public Vector2Int canvasSize = new Vector2Int(32, 32);
    public Vector2 pivot = new Vector2(0.5f, 0.5f);
    private HashSet<Vector2Int> dirtyPixels = new HashSet<Vector2Int>();
    private Texture2D cachedTexture;
    public CommandManager CommandManager { get; private set; } = new CommandManager();
    
    private Color[,] pixels;
    private Dictionary<Vector2Int, Color> previewPixels = new Dictionary<Vector2Int, Color>();
    public Dictionary<Vector2Int, Color> PreviewPixels => previewPixels;

    public PixelArtCanvas(Vector2Int canvasSize)
    {
        this.canvasSize = canvasSize;
        this.Initialize();
    }

    public void Initialize()
    {
        pixels = new Color[canvasSize.x, canvasSize.y];
        for (int x = 0; x < canvasSize.x; x++)
        {
            for (int y = 0; y < canvasSize.y; y++)
            {
                pixels[x, y] = Color.clear;
                dirtyPixels.Add(new Vector2Int(x, y));
            }
        }

        cachedTexture = new Texture2D(canvasSize.x, canvasSize.y, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        CommandManager.Clear();
    }
    
    public void SetPixel(int x, int y, Color color)
    {
        if (IsInBounds(x, y) && pixels[x, y] != color)
        {
            pixels[x, y] = color;
            dirtyPixels.Add(new Vector2Int(x, y));
        }
    }
    
    public Color GetPixel(int x, int y)
    {
        return IsInBounds(x, y) ? pixels[x, y] : Color.clear;
    }
    public Sprite RenderToSprite()
    {
        if (cachedTexture == null) Initialize(); // Safety check
        
        if (dirtyPixels.Count > 0)
        {
            foreach (var pixel in dirtyPixels)
            {
                cachedTexture.SetPixel(pixel.x, pixel.y, pixels[pixel.x, pixel.y]);
            }

            dirtyPixels.Clear();
        }

        if (previewPixels.Count > 0)
        {
            foreach (Vector2Int pixelPos in previewPixels.Keys)
            {
                cachedTexture.SetPixel(pixelPos.x, pixelPos.y, previewPixels[pixelPos]);
            }
        }
        cachedTexture.Apply();
        return Sprite.Create(cachedTexture, new Rect(0, 0, canvasSize.x, canvasSize.y), pivot);
    }
    
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < canvasSize.x && y >= 0 && y < canvasSize.y;
    }
}