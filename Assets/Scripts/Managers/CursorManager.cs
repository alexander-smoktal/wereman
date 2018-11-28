using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public enum CursorType
    {
        None = -1,
        Arrow,
        Walk,
        Stop
    }

    CursorType currentCursor = CursorType.None;
    public Texture2D arrowCursor;
    public Texture2D walkCursor;
    public Texture2D stopCursor;

    void Awake()
    {
        SetCursor(CursorType.Walk);
    }

    public void SetCursor(CursorType type)
    {
        if (currentCursor == type)
        {
            return;
        }

        currentCursor = type;

        switch (type)
        {
            case CursorType.Arrow:
                Cursor.SetCursor(arrowCursor, new Vector2(0, 0), CursorMode.Auto);
                break;
            case CursorType.Walk:
                Cursor.SetCursor(walkCursor, new Vector2(32, 32), CursorMode.Auto);
                break;
            case CursorType.Stop:
                Cursor.SetCursor(stopCursor, new Vector2(32, 32), CursorMode.Auto);
                break;

            default:
                Debug.Assert(false, "Invalid Cursor");
                break;
        }
        
    }
}
