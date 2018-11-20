using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {
    [Header("Painter")]
    public CellPainter painterPrefab;

    [Header("Colors")]
    static public Color clickColor = Color.red;
    static public Color defaultColor = Color.white;
    static public Color hoverColor = Color.magenta;
    static public Color neighbourColor = Color.green;

    [Header("Global settings")]
    public HexCoordinates coordinates;
    public Color color;
    // Original cell color. Should be changed to cell type color
    public Color originalColor = defaultColor;
    public int row;
    public int column;
    public int firstVertex;

    [Header("Path drawing sprites")]
    public Sprite activeSprite;
    public Sprite outlineSprite;
    public Sprite pathSprite;

    CellType type;
    CellPainter painter;

    void Awake()
    {
        int val = (int) (UnityEngine.Random.value * 100);
        if (val % 10 == 0)
        {
            type = new CellType(CellType.Type.Sand | CellType.Type.Stone);
        }
        else
        {
            type = new CellType(CellType.Type.Sand);
        }
        painter = Instantiate(painterPrefab);
        painter.Init(type);
        painter.transform.SetParent(transform, false);
    }

    public static class Geometry {
		public const float outerRadius = 10f;
		public const float innerRadius = outerRadius * 0.866025404f;

		public static Vector3[] corners = {
			new Vector3(0f, 0f, outerRadius),
			new Vector3(innerRadius, 0f, 0.5f * outerRadius),
			new Vector3(innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(0f, 0f, -outerRadius),
			new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius)
        };

        public static Bounds bounds = new Bounds(Vector3.zero, new Vector3(innerRadius, 0, outerRadius));
	}

    public bool IsPointInside(Vector3 point)
    {
        Vector3 position = transform.position;
        return Math.Abs(position.x - point.x) < Geometry.innerRadius
            && Math.Abs(position.y - point.y) < Geometry.innerRadius;
    }

    public bool IsPassable()
    {
        return type.IsPassable();
    }

    public void Draw(Sprite sprite)
    {
        painter.Draw(sprite);
    }

    public void Clear()
    {
        painter.Clear();
    }

    public void SetType(CellType.Type cellType)
    {
        if (type.Get() != cellType)
        {
            type = new CellType(cellType);
            painter.Init(type);
        }
    }
}
