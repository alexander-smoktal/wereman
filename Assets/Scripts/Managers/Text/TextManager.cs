using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : SingletonMonoBehaviour<TextManager>
{
    #region Constants
    public const EventManager.Events UpdateTextEvent = EventManager.Events.UpdateText;

    const string m_DefaultFilePath = "Text/English";
    const string m_DefaultPlaceHolder = "[TextPlaceHolder]";
    #endregion

    #region Serializable Fields
    [SerializeField] private string m_TextFilesDirectory = "";
    [SerializeField] private string[] m_TextFiles = null;
    #endregion

    #region Members
    private int m_FileIndex = 0;
    private string m_FilePath = m_DefaultFilePath;

    private Dictionary<string, string> m_Strings = new Dictionary<string, string>();
    #endregion

    #region MonoBehaviour
    protected new void Awake()
    {
        base.Awake();

        if(IsIndexValid(m_FileIndex))
            ChangeTextFile(m_FileIndex);
        else
            LoadFile(m_DefaultFilePath);
    }
    #endregion

    #region Events
    private void UpdateListeners()
    {
        EventManager.Instance.TrigerEvent(UpdateTextEvent);
    }
    #endregion

    #region Getters
    private bool IsIndexValid(int _index)
    {
        return (m_TextFiles != null) && (_index >= 0) && (_index < m_TextFiles.Length);
    }

    public int GetTextFileIndex()
    {
        return m_FileIndex;
    }

    public int GetTextFilesCount()
    {
        return m_TextFiles != null ? m_TextFiles.Length : 0;
    }

    public string GetTextFile(int _index)
    {
        Debug.Assert(IsIndexValid(_index), "Invalid index");

        return m_TextFiles[_index];
    }

    public string GetString(string _name)
    {
        string text;
        if (!m_Strings.TryGetValue(_name, out text))
            text = m_DefaultPlaceHolder + _name;

        return text;
    }
    #endregion

    #region Resource
    public void ChangeTextFile(int _index)
    {
        Debug.Assert(IsIndexValid(_index), "Invalid index");

        if (IsIndexValid(_index))
        {
            m_FilePath = m_TextFilesDirectory + m_TextFiles[_index];
            m_FileIndex = _index;

            LoadFile(m_FilePath);

            UpdateListeners();
        }
    }

    private void LoadFile(string _fileName)
    {
        m_Strings.Clear();

        string fileString = LoadTextFromAsset(_fileName);

        const char sepatator = '\n';
        string[] lines = fileString.Split(sepatator);
        foreach(string line in lines)
        {
            ParseLine(line);
        }
    }

    private string LoadTextFromAsset(string _fileName)
    {
        string result = "";
        TextAsset textAsset = Resources.Load<TextAsset>(_fileName);
        Debug.Assert(textAsset != null, "Invalid file name");

        if (textAsset != null)
            result = textAsset.text;

        return result;
    }
    #endregion

    #region Parser
    private void ParseLine(string _line)
    {
        const char sepatator = '=';
        int separatorIndex =_line.IndexOf(sepatator);

        if (separatorIndex == -1)
            return;

        string key = _line.Substring(0, separatorIndex);
        string val = _line.Substring(separatorIndex + 1);

        key = PurifyString(key);
        val = PurifyString(val);

        val = UpdateLiterals(val);

        Debug.Assert(key.Length > 0, "Empty key");
        Debug.Assert(!m_Strings.ContainsKey(key), "Duplication of a string [" + key + "]");

        m_Strings[key] = val;
    }

    private string PurifyString(string _line)
    {
        char[] skippable = {' ', '\r', '\t'};

        int lineBegin = 0;
        int lineEnd = 0;

        for (int i = 0; i < _line.Length; ++i)
        {
            if (!Array.Exists(skippable, ch => { return ch == _line[i]; }))
            {
                lineBegin = i;
                break;
            }
        }

        for (int i = _line.Length - 1; i >= 0; --i)
        {
            if (!Array.Exists(skippable, ch => { return ch == _line[i]; }))
            {
                lineEnd = i + 1;
                break;
            }
        }

        return _line.Substring(lineBegin, lineEnd - lineBegin);
    }
    
    private string UpdateLiterals(string _line)
    {
        Pair<string, string>[] replacePairs = new Pair<string, string>[]
            {
                new Pair<string, string>("\\\\", "\\"),
                new Pair<string, string>("\\n", "\n"),
                new Pair<string, string>("\\t", "\t"),
            };

        foreach(Pair<string, string> pair in replacePairs)
            _line = _line.Replace(pair.Key, pair.Value);

        return _line;
    }
    #endregion
}
