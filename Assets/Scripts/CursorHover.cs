using UnityEngine;

public class CheeseCursorHover : MonoBehaviour
{
    [Header("Cursor Settings")]
    public Texture2D hoverCursor; 
    
    public Vector2 hotSpot = Vector2.zero; 

    void OnMouseEnter()
    {
        Cursor.SetCursor(hoverCursor, hotSpot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}