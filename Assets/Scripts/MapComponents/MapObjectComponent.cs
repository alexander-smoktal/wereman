using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapObjectComponent : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField, HideInInspector] private List<HexCell> m_AttachedCells = null;
    [SerializeField] private bool m_IsObstacle = false;
    #endregion

    #region Debug Members
    [Header("Debug")]
    [SerializeField] private Color m_DebugAttachedCellColor = Color.red;
    #endregion

    #region Members
    private HexGrid    m_HexGrid  = null;
    private Collider2D m_Collider = null;
    #endregion

    #region Properties
    protected HexGrid hexGrid
    { get { return m_HexGrid; } }

    protected new Collider2D collider
    { get { return m_Collider; } }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        m_HexGrid = FindObjectOfType<HexGrid>();
        m_Collider = GetComponent<Collider2D>();
    }
    #endregion

    public void AttachCells(List<HexCell> cells)
    {
        if (m_IsObstacle)
            ClearObstacles();

        m_AttachedCells = cells;

        if (m_IsObstacle)
            SetObstacles();
    }

    private void ClearObstacles()
    {
        foreach (var cell in m_AttachedCells)
        {
            cell.RemoveType(CellType.Type.Obstacle);
        }
    }

    private void SetObstacles()
    {
        foreach(var cell in m_AttachedCells)
        {
            cell.AddType(CellType.Type.Obstacle);
        }
    }

    #region Debug
    protected void DrawDebugInfo()
    {
        if (!Selection.Contains(gameObject))
            return;

        DebugDrawCells(m_AttachedCells, m_DebugAttachedCellColor);
    }

    protected void DebugDrawCells(List<HexCell> cells, Color color)
    {
        foreach (var cell in cells)
        {
            PolygonCollider2D polygon = cell.GetCollider() as PolygonCollider2D;

            var middle = polygon.transform.position;

            for (int i = 0; i < polygon.points.Length; ++i)
            {
                var p0 = polygon.points[i];
                var p1 = polygon.points[(i + 1) % polygon.points.Length];

                p0 = polygon.transform.TransformPoint(p0);
                p1 = polygon.transform.TransformPoint(p1);

                Debug.DrawLine(p0, p1,     color);
                Debug.DrawLine(p0, middle, color);
            }
        }
    }
    #endregion
}