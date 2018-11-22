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

    #region Serialization
    private string GetDataPath()
    {
        string dataPath = Application.dataPath;

        int prefixIndex = dataPath.Length - c_DataPathAssets.Length;
        string prefix = dataPath.Substring(prefixIndex);

        if(prefix == c_DataPathAssets)
            return dataPath.Substring(0, prefixIndex);

        return dataPath;
    }

    public void Save(TextAsset mapAsset)
    {
        string assetPath = GetDataPath() + Path.AltDirectorySeparatorChar + AssetDatabase.GetAssetPath(mapAsset);
        SaveToFile(assetPath);
    }

    public void Load(TextAsset mapAsset)
    {
        Debug.Assert(mapAsset.bytes != null, "Non binary text asset");
        LoadFromBytes(mapAsset.bytes);
    }

    private void SaveToFile(string path)
    {

    }

    private void LoadFromBytes(byte[] bytesArray)
    {

    }
    #endregion
}
