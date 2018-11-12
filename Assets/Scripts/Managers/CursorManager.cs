using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CursorManager : MonoBehaviour {
    public enum CursorType
    {
        Walk,
        Stop
    }

    CursorType currentCursor;
    public Texture2D walkCursor;
    public Texture2D stopCursor;

    void Awake()
    {
        currentCursor = CursorType.Walk;
        Cursor.SetCursor(walkCursor, Vector2.zero, CursorMode.Auto);
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
            case CursorType.Walk:
                Cursor.SetCursor(walkCursor, Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.Stop:
                Cursor.SetCursor(stopCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
        
    }
}
