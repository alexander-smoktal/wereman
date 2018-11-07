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
    HexCell[] cells;
    MeshCollider coll;

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
  
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.RecalculateNormals();
        coll.sharedMesh = hexMesh;
    }

    void Triangulate(HexCell cell)
    {
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

    void OnMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                    {
                        closest.cell.color = closest.cell.originalColor;
                        closest.cell = cell;
                        closest.distance = cellDistance;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        cell.color = HexCell.clickColor;
                    }
                    else
                    {
                        cell.color = HexCell.hoverColor;
                    } 
                }
                else
                {
                    cell.color = cell.originalColor;
                }
            }

            if (Input.GetMouseButton(0))
            {
                HexGrid grid = transform.parent.gameObject.GetComponent<HexGrid>();
                List<HexCell> neighbors = grid.GetNeighbours(closest.cell);

                foreach (HexCell neighbour in neighbors)
                {
                    neighbour.color = HexCell.neighbourColor;
                }
            }

            Triangulate(cells);
        }
    }
}
