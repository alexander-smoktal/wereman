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

public class HexGrid : MonoBehaviour
{
    [SerializeField] private TextAsset mapAsset = null;

    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;

    Canvas gridCanvas;
    HexMesh hexMesh;
    HexCell[] cells;
    HexCell selectedCell;

    MapSerializer mapSerializer = null;

    int GetCellIndex(int column, int row)
    {
        return row * width + column;
    }

    void LoadMap()
    {
        if(mapAsset != null)
        {
            mapSerializer = new MapSerializer();
            mapSerializer.Load(mapAsset);

            if(!mapSerializer.IsInitialized())
                Debug.LogWarning("Failed to load map");
            else if ((mapSerializer.Width != width) || (mapSerializer.Height != height))
                Debug.LogWarning("Loaded map is incompatible");
        }
    }

    public void SaveMap()
    {
        Debug.Assert((mapAsset != null), "Invalid map asset");
        if (mapAsset != null)
        {
            mapSerializer = new MapSerializer(width, height);

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int index = GetCellIndex(column, row);
                    HexCell cell = cells[index];

                    MapSerializer.CellProperties cellProperties = new MapSerializer.CellProperties();
                    cellProperties.m_Type = cell.GetType();

                    mapSerializer.SetValue(column, row, cellProperties);
                }
            }

            mapSerializer.Save(mapAsset);
        }
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void Awake()
    {
        LoadMap();
        bool isLoaded = (mapSerializer != null) && mapSerializer.IsInitialized();

        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                CreateCell(column, row, isLoaded);
            }
        }

        mapSerializer = null;
    }

    void CreateCell(int column, int row, bool load)
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

        if(load)
        {
            MapSerializer.CellProperties cellProperties = mapSerializer.GetValue(column, row);
            cell.SetType(cellProperties.m_Type);
        }

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
    // Reconstruct A* path
    List<HexCell> ASReconstructPath(HexCell cell, Dictionary<HexCell, HexCell> cameFrom)
    {
        List<HexCell> result = new List<HexCell>();
        result.Add(cell);

        while (cameFrom.ContainsKey(cell))
        {
            cell = cameFrom[cell];
            result.Add(cell);
        }

        return result;
    }

    // A*
    public List<HexCell> MakePath(HexCell from, HexCell to)
    {
        Dictionary<HexCell, float> openSet = new Dictionary<HexCell, float>
        {
            { from, Vector3.Distance(from.transform.position, to.transform.position) }
        };

        Dictionary<HexCell, float> closedSet = new Dictionary<HexCell, float>
        {
            { from, 0 }
        };

        Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();

        while (openSet.Count > 0)
        {
            HexCell current = from;
            float minDist = float.MaxValue;
            // Find cell with min distance
            foreach (KeyValuePair<HexCell, float> cell in openSet)
            {
                if (cell.Value < minDist)
                {
                    current = cell.Key;
                    minDist = cell.Value;
                }
            }

            if (current == to)
            {
                return ASReconstructPath(current, cameFrom);
            }

            openSet.Remove(current);

            if (!closedSet.ContainsKey(current))
            {
                closedSet[current] = float.MaxValue;
            }

            foreach (HexCell neighbour in GetNeighbours(current))
            {
                if (!neighbour.IsPassable() || closedSet.ContainsKey(neighbour))
                {
                    continue;
                }

                // This heuristics give better result than the original because of non-linear space
                float tentative = closedSet[current] +
                    Vector3.Distance(current.transform.position, neighbour.transform.position);

                if (!openSet.ContainsKey(neighbour))
                {
                    openSet.Add(neighbour, float.MaxValue);
                }
                else if (closedSet.ContainsKey(neighbour) &&
                    tentative >= closedSet[neighbour])
                {
                    continue;
                }

                cameFrom[neighbour] = current;
                closedSet[neighbour] = tentative;
                openSet[neighbour] = tentative + Vector3.Distance(to.transform.position, neighbour.transform.position);
            }
        }

        // Not found!
        return new List<HexCell>();
    }

    public Vector2Int GetCell(Ray ray)
    {
        return hexMesh.GetCell(ray);
    }

    public void OverUI()
    {
        GetComponentInChildren<CursorManager>().SetCursor(CursorManager.CursorType.Arrow);
    }

    public void HiglightCell(int column, int row)
    {
        int index = GetCellIndex(column, row);
        HexCell cell = cells[index];

        if (cell == selectedCell)
        {
            return;
        }

        RemoveSelection();

        if (selectedCell)
        {
            List<HexCell> path = MakePath(selectedCell, cell);

            // Remove active cell
            if (path.Count > 0)
            {
                path.RemoveAt(path.Count - 1);
            }
            
            foreach (HexCell pathCell in path)
            {
                pathCell.Draw(cell.pathSprite);
            }
        }

        if (cell.IsPassable())
        {
            GetComponentInChildren<CursorManager>().SetCursor(CursorManager.CursorType.Walk);
            cell.Clear();
            cell.Draw(cell.outlineSprite);
        }
        else
        {
            GetComponentInChildren<CursorManager>().SetCursor(CursorManager.CursorType.Stop);
        }
        
        //hexMesh.HiglightCell(cell);
    }

    public void RemoveHiglighting()
    {
        RemoveSelection();
        //hexMesh.RemoveHiglighting();
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

        if (!cell.IsPassable())
        {
            return;
        }

        selectedCell = cell;
        cell.Draw(cell.activeSprite);
        RemoveSelection();
        //hexMesh.SelectCell();
    }

    public void RemoveSelection()
    {
        foreach (HexCell cell in cells)
        {
            cell.Clear();
        }

        if (selectedCell)
        {
            selectedCell.Draw(selectedCell.activeSprite);
        }
        // hexMesh.RemoveHiglighting();
    }

    #region Editor
    public void SetEditorSelection(int column, int row)
    {
        // TO DO - Higlight selected cells
    }

    public void SetCellProperties(int column, int row, InGameEditor.Properties editorProperties)
    {
        int index = GetCellIndex(column, row);
        HexCell cell = cells[index];

        CellType.Type cellType = editorProperties.GetCellType();
        cell.SetType(cellType);
    }
    #endregion
}
