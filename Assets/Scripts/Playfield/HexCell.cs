using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    [Header("Painter")]
    public CellPainter painterPrefab;

    [Header("Collider")]
    public HexCellMeshCollider    colliderPrefab;
    public HexCellPolygonCollider collider2DPrefab;

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

    CellType type = null;
    CellPainter painter = null;
    HexCellMeshCollider cellCollider = null;
    HexCellPolygonCollider cellCollider2D = null;

    void Awake()
    {
        if (type == null)
        {
            int val = (int)(UnityEngine.Random.value * 100);
            if (val % 10 == 0)
            {
                type = new CellType(CellType.Type.Sand | CellType.Type.Stone);
            }
            else
            {
                type = new CellType(CellType.Type.Sand);
            }
        }

        painter = Instantiate(painterPrefab);
        painter.Init(type);
        painter.transform.SetParent(transform, false);

        cellCollider = Instantiate(colliderPrefab, transform, false);

        Vector3 colliderLocalScale  = cellCollider.transform.localScale;
        Vector3 colliderGlobalScale = cellCollider.transform.lossyScale;
        cellCollider.transform.localScale = new Vector3(colliderLocalScale.x / colliderGlobalScale.x, colliderLocalScale.y / colliderGlobalScale.y, colliderLocalScale.z / colliderGlobalScale.z);

        cellCollider2D = Instantiate(collider2DPrefab, transform, false);

        Vector3 collider2DLocalScale  = cellCollider2D.transform.localScale;
        Vector3 collider2DGlobalScale = cellCollider2D.transform.lossyScale;
        cellCollider2D.transform.localScale = new Vector3(collider2DLocalScale.x / collider2DGlobalScale.x, collider2DLocalScale.y / collider2DGlobalScale.y, collider2DLocalScale.z / collider2DGlobalScale.z);
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

    new public CellType.Type GetType()
    {
        return type.Get();
    }

    public void SetType(CellType.Type cellType)
    {
        if ((type == null) || (type.Get() != cellType))
        {
            type = new CellType(cellType);

            if(painter != null)
                painter.Init(type);
        }
    }

    public void AddType(CellType.Type cellType)
    {
        CellType.Type newtype = type.Get() | cellType;
        SetType(newtype);
    }

    public void RemoveType(CellType.Type cellType)
    {
        CellType.Type newtype = type.Get() & ~cellType;
        SetType(newtype);
    }

    public Collider2D GetCollider()
    {
        return cellCollider2D.GetCollider();
    }
}
