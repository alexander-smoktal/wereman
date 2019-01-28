using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCellPolygonCollider : MonoBehaviour
{
    #region Constants
    private const int c_VerticesCount = 6;
    #endregion

    #region Members
    private PolygonCollider2D m_Collider;

    private Vector2[] m_Vertices = new Vector2[c_VerticesCount];
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        m_Collider = GetComponent<PolygonCollider2D>();
        Debug.Assert(m_Collider != null, "Failed to get PolygonCollider2D component");

        InitPolygon();
    }

    void Start()
    {
    }
    #endregion

    #region Mesh Initialization
    void InitPolygon()
    {
        if (m_Collider == null)
        {
            Debug.LogError("Invalid collider component");
            return;
        }

        InitVertices();

        m_Collider.points = m_Vertices;
    }

    Vector2 GetVertex(int index)
    {
        var vertex = HexCell.Geometry.corners[index];

        return new Vector2(vertex.x, vertex.z);
    }

    void InitVertices()
    {
        for (int i = 0; i < c_VerticesCount; i++)
        {
            m_Vertices[i] = GetVertex(i);
        }
    }
    #endregion

    #region Collider
    public Collider2D GetCollider()
    {
        return m_Collider;
    }
    #endregion
}
