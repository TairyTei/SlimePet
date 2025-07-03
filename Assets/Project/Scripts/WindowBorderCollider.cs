using UnityEngine;

public class WindowBorderCollider : MonoBehaviour
{
    public float borderWidth = 1;
    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;
    public BoxCollider2D topCollider;
    public BoxCollider2D bottomCollider;

    public Vector2 LeftBottomCameraCorner;
    public Vector2 RightTopCameraCorner;

    public Vector2 camAreaSize;

    public static WindowBorderCollider instance;
    public void Start()
    {
        instance = this;
        if (!leftCollider) leftCollider = gameObject.AddComponent<BoxCollider2D>();
        if (!rightCollider) rightCollider = gameObject.AddComponent<BoxCollider2D>();
        if (!topCollider) topCollider = gameObject.AddComponent<BoxCollider2D>();
        if (!bottomCollider) bottomCollider = gameObject.AddComponent<BoxCollider2D>();

        UpdateBorders();
    }

    public void UpdateBorders()
    {
        LeftBottomCameraCorner = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        RightTopCameraCorner = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        camAreaSize = new Vector2(RightTopCameraCorner.x - LeftBottomCameraCorner.x, RightTopCameraCorner.y - LeftBottomCameraCorner.y);
        
        bottomCollider.offset = new Vector2(0, LeftBottomCameraCorner.y - borderWidth/2);
        bottomCollider.size = new Vector2(camAreaSize.x, borderWidth);

        topCollider.offset = new Vector2(0, RightTopCameraCorner.y + borderWidth / 2);
        topCollider.size = new Vector2(camAreaSize.x, borderWidth);

        leftCollider.offset = new Vector2(LeftBottomCameraCorner.x - borderWidth/2, 0);
        leftCollider.size = new Vector2(borderWidth, camAreaSize.y);

        rightCollider.offset = new Vector2(RightTopCameraCorner.x + borderWidth / 2, 0);
        rightCollider.size = new Vector2(borderWidth, camAreaSize.y);
    }
    public Vector2 GetRandomPointInRoom(){
        Vector2 randomTarget = new Vector2(Random.Range(LeftBottomCameraCorner.x, RightTopCameraCorner.x),
        Random.Range(LeftBottomCameraCorner.y, RightTopCameraCorner.y));
        return randomTarget;
    }
    public bool isInBounds(Vector2 point){
        if(point.x > RightTopCameraCorner.x) return false;
        if(point.x < LeftBottomCameraCorner.x) return false;
        if(point.y > RightTopCameraCorner.y) return false;
        if(point.y < LeftBottomCameraCorner.y) return false;
        return true;
    }
}