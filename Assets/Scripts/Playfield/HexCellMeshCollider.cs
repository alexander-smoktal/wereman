using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCellMeshCollider : MonoBehaviour
{
    #region Constants
    private const int c_VerticesCount  = 6;

    private const int c_TrianglesCount = 4;
    private const int c_IndicesCount   = c_TrianglesCount * 3;

    public const int c_RaycastLayer = 1 << 9;
    #endregion

    #region Members
    private Mesh          m_Mesh;
    private MeshCollider  m_Collider;
    /*
#if DEBUG
    private MeshFilter   m_MeshFilter;
    private MeshRenderer m_MeshRenderer;
#endif
*/
    private Vector3[] m_Vertices  = new Vector3[c_VerticesCount];
    private int[]     m_Triangles = new int[c_IndicesCount];
    private Color[]   m_Colors    = new Color[c_VerticesCount];
    #endregion

    #region MonoBehaviour
    void Awake ()
    {
        m_Mesh = new Mesh();
        m_Mesh.name = "Hex Mesh";

        m_Collider = GetComponent<MeshCollider>();
        Debug.Assert(m_Collider != null, "Failed to get MeshCollider component");
        /*
#if DEBUG
        m_MeshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        Debug.Assert(m_MeshRenderer != null, "Failed to add MeshRenderer component");

        m_MeshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        Debug.Assert(m_MeshFilter != null, "Failed to add MeshFilter component");
        if (m_MeshFilter != null)
            m_MeshFilter.mesh = m_Mesh;
#endif*/
    }

    void Start()
    {
        InitMesh();
    }
    #endregion

    #region Mesh Initialization
    void InitMesh()
    {
        if (m_Mesh == null)
        {
            Debug.LogError("Invalid mesh");
            return;
        }

        if (m_Collider == null)
        {
            Debug.LogError("Invalid collider component");
            return;
        }

        Triangulate();

        m_Mesh.vertices  = m_Vertices;
        m_Mesh.triangles = m_Triangles;
        m_Mesh.colors    = m_Colors;
        m_Mesh.RecalculateNormals();

        m_Collider.sharedMesh = m_Mesh;
    }

    void Triangulate()
    {
        InitVertices();
        InitTriangles();
        InitColors();
    }

    Vector3 GetVertex(int index)
    {
        var vertex = HexCell.Geometry.corners[index];

        return new Vector3(vertex.x, vertex.z, vertex.y);
    }

    void InitVertices()
    {
        for (int i = 0; i < c_VerticesCount; i++)
        {
            m_Vertices[i] = GetVertex(i);
        }
    }

    void InitTriangles()
    {
        for (int i = 0; i < c_TrianglesCount; i++)
        {
            int firstIndex = i * 3;

            m_Triangles[firstIndex]     = 0;
            m_Triangles[firstIndex + 1] = i + 1;
            m_Triangles[firstIndex + 2] = i + 2;
        }
    }

    void InitColors()
    {
        for (int i = 0; i < c_VerticesCount; i++)
        {
            m_Colors[i] = new Color(0, 0, 0, 0);
        }
    }
    #endregion
}
