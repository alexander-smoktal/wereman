using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;

public class MapSerializer
{
    #region Types
    [System.Serializable]
    public struct MapProperties
    {
        public int m_Width;
        public int m_Height;
    }

    [System.Serializable]
    public struct CellProperties
    {
        public CellType.Type m_Type;
    }
    #endregion

    #region Constants
    const string c_DataPathAssets = "/Assets";
    #endregion

    #region Members
    private MapProperties m_Properties = new MapProperties { m_Width = 0, m_Height = 0 };
    private CellProperties[] m_CellsProperties = null;
    #endregion

    #region Properties
    public int Width
    { get { return m_Properties.m_Width; } }

    public int Height
    { get { return m_Properties.m_Height; } }
    #endregion

    #region Constructors
    public MapSerializer()
    {

    }

    public MapSerializer(int width, int height)
    {
        Resize(width, height);
    }

    public void Resize(int width, int height)
    {
        int count = width * height;

        if ((m_CellsProperties == null) || (count != m_CellsProperties.Length))
        {
            m_CellsProperties = new CellProperties[count];
        }

        m_Properties.m_Width = width;
        m_Properties.m_Height = height;
    }
    #endregion

    #region Values
    private int GetCellIndex(int column, int row)
    {
        return row * Width + column;
    }

    public void SetValue(int column, int row, CellProperties cell)
    {
        int index = GetCellIndex(column, row);

        m_CellsProperties[index] = cell;
    }

    public CellProperties GetValue(int column, int row)
    {
        bool invalidColumn = (column < 0) || (column > Width);
        bool invalidRow    = (row < 0) || (row > Height);

        if(invalidColumn || invalidRow)
        {
            CellProperties emptyCell = new CellProperties
                {
                    m_Type = CellType.Type.None,
                };

            return emptyCell;
        }

        int index = GetCellIndex(column, row);

        return m_CellsProperties[index];
    }
    #endregion

    #region Serialization
    public void Save(TextAsset mapAsset)
    {
        string assetPath = AssetDatabase.GetAssetPath(mapAsset);
        SaveToFile(assetPath);
    }

    public void Load(TextAsset mapAsset)
    {
        // Asset doesn't reload immediately after saving.
        // Load via file stream in editor as workaround
        if (Application.isEditor)
        {
            string assetPath = AssetDatabase.GetAssetPath(mapAsset);
            LoadFromFile(assetPath);
        }
        else
        {
            LoadFromBytes(mapAsset.bytes);
        }

        ValidateMap();
    }

    private void ValidateMap()
    {
        int size = Width * Height;

        if (size != m_CellsProperties.Length)
        {
            Debug.LogError("Inconsisten map properties");

            m_Properties.m_Width  = 0;
            m_Properties.m_Height = 0;

            m_CellsProperties = null;
        }
    }

    public bool IsInitialized()
    {
        return m_CellsProperties != null;
    }

    private void SaveToFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Truncate);

        SaveMap(fileStream);

        fileStream.Close();
    }

    private void SaveMap(Stream stream)
    {
        SavePrefix(stream);
        SaveCells(stream);
    }

    private void SavePrefix(Stream stream)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(stream, m_Properties);
    }

    private void SaveCells(Stream stream)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(stream, m_CellsProperties);
    }

    private void LoadFromBytes(byte[] bytesArray)
    {
        if (bytesArray.Length == 0)
        {
            Debug.LogWarning("Empty map file");
            return;
        }

        MemoryStream memoryStream = new MemoryStream(bytesArray);

        LoadMap(memoryStream);

        memoryStream.Close();
    }

    private void LoadFromFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open);

        LoadMap(fileStream);

        fileStream.Close();
    }

    private void LoadMap(Stream stream)
    {
        LoadPrefix(stream);
        LoadCells(stream);
    }

    private void LoadPrefix(Stream stream)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        m_Properties = (MapProperties)binaryFormatter.Deserialize(stream);
    }

    private void LoadCells(Stream stream)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        m_CellsProperties = (CellProperties[])binaryFormatter.Deserialize(stream);
    }
    #endregion
}
