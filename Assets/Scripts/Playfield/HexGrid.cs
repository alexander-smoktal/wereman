using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X { get { return x; }}
    public int Y { get { return -X - Z; } }
    public int Z { get { return z; }}

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}

public class HexGrid : MonoBehaviour {
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;

    Canvas gridCanvas;
    HexMesh hexMesh;
    HexCell[] cells;

    int GetCellIndex(int column, int row)
    {
        return row * width + column;
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                CreateCell(column, row);
            }
        }
    }

    void CreateCell(int column, int row)
    {
        Vector3 position;
        position.x = (column + row * 0.5f - row / 2) * (HexCell.Geometry.innerRadius * 2f);
        position.y = 0f;
        position.z = row * (HexCell.Geometry.outerRadius * 1.5f);

        HexCell cell = cells[GetCellIndex(column, row)] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(column, row);
        cell.color = HexCell.defaultColor;
        cell.row = row;
        cell.column = column;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    public List<HexCell> GetNeighbours(HexCell cell)
    {
        List<HexCell> result = new List<HexCell>();

        if (cell.column > 0)
        {
            result.Add(cells[GetCellIndex(cell.column - 1, cell.row)]);
        }

        if (cell.column < width - 1)
        {
            result.Add(cells[GetCellIndex(cell.column + 1, cell.row)]);
        }

        if (cell.row > 0)
        {
            result.Add(cells[GetCellIndex(cell.column, cell.row - 1)]);

            // Shifted
            if (cell.row % 2 == 0 && cell.column > 0)
            {
                result.Add(cells[GetCellIndex(cell.column - 1, cell.row - 1)]);
            }

            if (cell.row % 2 != 0 && cell.column < width - 1)
            {
                result.Add(cells[GetCellIndex(cell.column + 1, cell.row - 1)]);
            }
        }

        if (cell.row < height - 1)
        {
            result.Add(cells[GetCellIndex(cell.column, cell.row + 1)]);

            // Shifted
            if (cell.row % 2 == 0 && cell.column > 0)
            {
                result.Add(cells[GetCellIndex(cell.column - 1, cell.row + 1)]);
            }

            if (cell.row % 2 != 0 && cell.column < width - 1)
            {
                result.Add(cells[GetCellIndex(cell.column + 1, cell.row + 1)]);
            }
        }


        return result;
    }

    public Vector2Int GetCell(Ray ray)
    {
        return hexMesh.GetCell(ray);
    }

    public void HiglightCell(int column, int row)
    {
        int index = GetCellIndex(column, row);
        HexCell cell = cells[index];

        hexMesh.HiglightCell(cell);
    }

    public void RemoveHiglighting()
    {
        hexMesh.RemoveHiglighting();
    }

    public Vector2Int higlightedCell
    {
        get
        {
            var cellIndex = new Vector2Int(-1, -1);

            var cell = hexMesh.higlightedCell;
            if (cell != null)
                cellIndex = new Vector2Int(cell.column, cell.row);

            return cellIndex;
        }
    }

    public void SelectCell(int column, int row)
    {
        int index = GetCellIndex(column, row);
        HexCell cell = cells[index];

        hexMesh.SelectCell(cell);
    }

    public void RemoveSelection()
    {
        hexMesh.RemoveHiglighting();
    }
}
