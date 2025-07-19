using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PixelArtEditor : MonoBehaviour
{
    [SerializeField] private Color[] availableColors;
    [SerializeField] private Transform colorsPaletteParent;
    [SerializeField] private GameObject colorButtonPrefab;
    [SerializeField] private Image canvasDisplay;
    [SerializeField] private CanvasInputHandler canvasInputHandler;
    
    private PixelArtTool activeTool;
    private PixelArtCanvas activeCanvas;
    private Color activeColor;

    private void Start()
    {

        if (activeCanvas == null)
        {
            activeCanvas = new PixelArtCanvas(new Vector2Int(64, 64));
        }
        ApplyToDisplay();
        DrawPreview();

        LoadColors();

        SetActiveTool(new Pencil_PixelArtTool());
    }
    void Update()
    {
        if (canvasInputHandler.IsPointerOnCanvas && activeTool != null
        || activeTool != null && activeTool.IsToolUsed) HandleCanvasInput();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
        else if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))
        {
            Redo();
        }

    }
    private void LoadColors()
    {
        while (colorsPaletteParent.childCount > 0)
        {
            Destroy(colorsPaletteParent.GetChild(0).gameObject);
        }
        foreach (Color color in availableColors)
        {
            GameObject colorButtonObject = Instantiate(colorButtonPrefab, colorsPaletteParent);
            ColorButton colorButton = colorButtonObject.GetComponent<ColorButton>();
            colorButton.Initialize(this, color);
        }

        if (availableColors.Length > 0)
        {
            activeColor = availableColors[0];
        }
        else
        {
            activeColor = Color.black;
        }
    }
    public void ApplyToDisplay()
    {
        //DestroyImmediate(canvasDisplay.sprite);
        canvasDisplay.sprite = activeCanvas.RenderToSprite();
    }
    public void DrawPreview()
    {
        ApplyToDisplay();
    }

    public void HandleCanvasInput()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasDisplay.rectTransform,
            Input.mousePosition,
            null, // Используем null вместо Camera.main для UI
            out localPoint
        );

        // Получаем размеры RectTransform
        Rect rect = canvasDisplay.rectTransform.rect;

        // Нормализуем координаты (от 0 до 1)
        float normalizedX = (localPoint.x - rect.x) / rect.width;
        float normalizedY = (localPoint.y - rect.y) / rect.height;

        // Переводим в координаты пикселей
        int x = Mathf.FloorToInt(normalizedX * activeCanvas.canvasSize.x);
        int y = Mathf.FloorToInt(normalizedY * activeCanvas.canvasSize.y);

        // Убедимся, что координаты в пределах холста
        x = Mathf.Clamp(x, 0, activeCanvas.canvasSize.x - 1);
        y = Mathf.Clamp(y, 0, activeCanvas.canvasSize.y - 1);

        activeTool.OnHover(activeCanvas, activeColor, x, y);
    }
    
    public void Undo()
    {
        activeCanvas.CommandManager.Undo(activeCanvas);
        ApplyToDisplay();
    }

    public void Redo()
    {
        activeCanvas.CommandManager.Redo(activeCanvas);
        ApplyToDisplay();
    }
    public void SetActiveTool(PixelArtTool tool)
    {
        if (activeTool != null)
        {
            activeTool.OnChanged -= ApplyToDisplay;
            activeTool.OnPreviewChanged -= DrawPreview;
        }
        activeTool = tool;
        if (activeTool != null)
        {
            activeTool.OnChanged += ApplyToDisplay;
            activeTool.OnPreviewChanged += DrawPreview;
        }

    }
    public void SetColor(Color color)
    {
        activeColor = color;
    }
}
