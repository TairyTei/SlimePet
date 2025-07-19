using UnityEngine;
using UnityEngine.EventSystems;
public class CanvasInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsPointerOnCanvas => isPointerOnCanvas;
    
    private bool isPointerOnCanvas = false;

    public void OnPointerEnter(PointerEventData eventData) => isPointerOnCanvas = true;
    public void OnPointerExit(PointerEventData eventData) => isPointerOnCanvas = false;
}