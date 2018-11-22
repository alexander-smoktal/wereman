using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapSerializer
{
    #region Constants
    const string c_DataPathAssets = "/Assets";
    #endregion

    #region Members
    private InGameEditor.Properties[] m_CellsProperties = null;

    private int m_Width  = 0;
    private int m_Height = 0;
    #endregion

    #region Properties
    public int Width
    { get { return m_Width; } }

    public int Height
    { get { return m_Height; } }
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
            m_CellsProperties = new InGameEditor.Properties[count];
        }

        m_Width = width;
        m_Height = height;
    }
    #endregion

    #region Values
    private int GetCellIndex(int column, int row)
    {
        return row * m_Width + column;
    }

    public void SetValue(int column, int row, InGameEditor.Properties cell)
    {
        int index = GetCellIndex(column, row);

        m_CellsProperties[index] = cell;
    }

    public InGameEditor.Properties GetValue(int column, int row)
    {
        bool invalidColumn = (column < 0) || (column > m_Width);
        bool invalidRow    = (row < 0) || (row > m_Height);

        if(invalidColumn || invalidRow)
        {
            InGameEditor.Properties emptyCell = new InGameEditor.Properties
                {
                    groundType = InGameEditor.GroundType.Invalid,
                    stone = false,
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
    }

    public bool IsInitialized()
    {
        return m_CellsProperties != null;
    }

    private void SaveToFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Truncate);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);

        SaveMap(binaryWriter);

        binaryWriter.Close();
        fileStream.Close();
    }

    private void SaveMap(BinaryWriter binaryWriter)
    {
        SavePrefix(binaryWriter);
        SaveCells(binaryWriter);
    }

    private void SavePrefix(BinaryWriter binaryWriter)
    {
        binaryWriter.Write(m_Width);
        binaryWriter.Write(m_Height);
    }

    private void SaveCells(BinaryWriter binaryWriter)
    {
        for (int i = 0; i < m_CellsProperties.Length; ++i)
        {
            SaveCell(binaryWriter, m_CellsProperties[i]);
        }
    }

    private void SaveCell(BinaryWriter binaryWriter, InGameEditor.Properties cell)
    {
        int groundType = (int)cell.groundType;

        binaryWriter.Write(groundType);
        binaryWriter.Write(cell.stone);
    }

    private void LoadFromBytes(byte[] bytesArray)
    {
        if (bytesArray.Length == 0)
        {
            Debug.LogWarning("Empty map file");
            return;
        }

        MemoryStream memoryStream = new MemoryStream(bytesArray);
        BinaryReader binaryReader = new BinaryReader(memoryStream);

        LoadMap(binaryReader);

        binaryReader.Close();
        memoryStream.Close();
    }

    private void LoadFromFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open);
        BinaryReader binaryReader = new BinaryReader(fileStream);

        LoadMap(binaryReader);

        binaryReader.Close();
        fileStream.Close();
    }

    private void LoadMap(BinaryReader binaryReader)
    {
        LoadPrefix(binaryReader);
        LoadCells(binaryReader);
    }

    private void LoadPrefix(BinaryReader binaryReader)
    {
        int width  = binaryReader.ReadInt32();
        int height = binaryReader.ReadInt32();

        Resize(width, height);
    }

    private void LoadCells(BinaryReader binaryReader)
    {
        for(int i = 0; i < m_CellsProperties.Length; ++i)
        {
            m_CellsProperties[i] = LoadCell(binaryReader);
        }
    }

    private InGameEditor.Properties LoadCell(BinaryReader binaryReader)
    {
        InGameEditor.Properties cell;

        cell.groundType = (InGameEditor.GroundType)binaryReader.ReadInt32();
        cell.stone      = binaryReader.ReadBoolean();

        return cell;
    }
    #endregion
}
