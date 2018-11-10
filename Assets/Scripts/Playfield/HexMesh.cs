using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexMesh : MonoBehaviour
{
    class CellDistancePair
    {
        public HexCell cell;
        public float distance;
    };

    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;
    List<Vector2> uv = new List<Vector2>();
    HexCell[] cells;
    MeshCollider coll;
    Bounds bounds;

    HexCell selected = null;
    HexCell hoverCell = null;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";

        coll = GetComponent<MeshCollider>();

        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
    }

    public void Triangulate(HexCell[] cellz)
    {
        cells = cellz;
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        bounds = new Bounds();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        ComputeUVs();

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.SetUVs(0, uv);
        hexMesh.RecalculateNormals();
        coll.sharedMesh = hexMesh;
    }

    void Triangulate(HexCell cell)
    {
        cell.firstVertex = vertices.Count;

        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                center,
                center + HexCell.Geometry.corners[i],
                center + HexCell.Geometry.corners[i + 1]
            );
            AddTriangleColor(cell.color);
        }

        var cellBounds = HexCell.Geometry.bounds;
        cellBounds.center = center;

        bounds.Encapsulate(cellBounds);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void ComputeUVs()
    {
        uv.Clear();

        for (int i = 0, count = vertices.Count; i < count; i++)
        {
            var v = vertices[i] - bounds.min;

            uv.Add(new Vector2(v.x / bounds.size.x, v.z / bounds.size.z));
        }
    }

    void UpdateMeshColors()
    {
        UpdateHoverColors();
        UpdateSelectedColors();

        hexMesh.SetColors(colors);
    }

    Color BlendColors(Color color0, Color color1)
    {
        Color blend = color0 + color1;
        return blend;
    }

    void SetCellCollor(HexCell cell, Color color, bool blend = false)
    {
        if (blend)
            color = BlendColors(cell.color, color);

        cell.color = color;

        for(int i = 0; i < 6*3; ++i)
        {
            colors[cell.firstVertex + i] = color;
        }
    }

    void SetNeighborsColor(HexCell cell, Color color, bool blend = false)
    {
        HexGrid grid = transform.parent.gameObject.GetComponent<HexGrid>();
        List<HexCell> neighbors = grid.GetNeighbours(cell);

        foreach (HexCell neighbour in neighbors)
        {
            neighbour.color = color;

            SetCellCollor(neighbour, color, blend && (neighbour == hoverCell));
        }
    }

    void SetSelectedCell(HexCell cell)
    {
        if (cell == selected)
            return;

        if (selected != null)
        {
            SetCellCollor(selected, selected.originalColor);
            SetNeighborsColor(selected, selected.originalColor);
        }
        
        selected = cell;
    }

    void UpdateSelectedColors()
    {
        if (selected != null)
        {
            SetCellCollor(selected, HexCell.clickColor, (selected == hoverCell));
            SetNeighborsColor(selected, HexCell.neighbourColor, true);
        }
    }

    void SetHoverCell(HexCell cell)
    {
        if (cell == hoverCell)
            return;

        if(hoverCell != null)
            SetCellCollor(hoverCell, hoverCell.originalColor);

        hoverCell = cell;
    }

    void UpdateHoverColors()
    {
        if (hoverCell != null)
            SetCellCollor(hoverCell, HexCell.hoverColor);
    }

    public Vector2Int GetCell(Ray ray)
    {
        Vector2Int cellIndex = new Vector2Int(-1, -1);

        RaycastHit hit;
        if (coll.Raycast(ray, out hit, 100.0f))
        {
            // Checking if point is inside the cell is hard op. We just find closest point
            CellDistancePair closest = new CellDistancePair();
            closest.cell = cells[0];
            closest.distance = Vector3.Distance(cells[0].transform.position, hit.point);

            foreach (HexCell cell in cells)
            {
                float cellDistance = Vector3.Distance(cell.transform.position, hit.point);

                if (cellDistance <= closest.distance)
                {
                    // Update closest point
                    closest.cell.color = closest.cell.originalColor;
                    closest.cell = cell;
                    closest.distance = cellDistance;
                }
            }

            cellIndex = new Vector2Int(closest.cell.column, closest.cell.row);
        }

        return cellIndex;
    }
    
    public void HiglightCell(HexCell cell)
    {
        Debug.Assert(cell != null, "Invalid cell");
        SetHoverCell(cell);

        UpdateMeshColors();
    }

    public void RemoveHiglighting()
    {
        SetHoverCell(null);

        UpdateMeshColors();
    }

    public void SelectCell(HexCell cell)
    {
        Debug.Assert(cell != null, "Invalid cell");
        SetSelectedCell(cell);

        UpdateMeshColors();
    }

    public void RemoveSelection()
    {
        SetSelectedCell(null);

        UpdateMeshColors();
    }
}
