using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoldablePanel : MonoBehaviour
{
    #region Serializable
    [Header("Components")]
    [SerializeField] private RectTransform m_Panel = null;

    [SerializeField] private Button m_ButtonShrink   = null;
    [SerializeField] private Button m_ButtonMinimize = null;
    [SerializeField] private Button m_ButtonExpand   = null;

    [Header("Size")]
    [SerializeField] private int     m_InitialSize     = 1;
    [SerializeField] private bool    m_MinimizeOnStart = true;
    [SerializeField] private float[] m_Sizes           = null;
    #endregion

    #region Members
    private int  m_SizeIndex = 0;
    private bool m_IsMinimized = false;
    #endregion

    #region MonoBehaviour
    void Start ()
    {
        m_IsMinimized = m_MinimizeOnStart;
        m_SizeIndex = m_InitialSize;

        UpdateSize();
	}
    #endregion

    #region Size
    void Resize(int index)
    {
        Debug.Assert((index >= 0) && (index < m_Sizes.Length), "Invalid size index");
        m_SizeIndex = index;

        UpdateSize();
    }

    void Minimize(bool minimized)
    {
        m_IsMinimized = minimized;

        UpdateSize();
    }

    void UpdateSize()
    {
        if (m_IsMinimized)
            ResizePanel(0);
        else
            ResizePanel(m_SizeIndex);
    }

    void ResizePanel(int index)
    {
        Debug.Assert(m_Panel != null, "Invalid Panel Component");
        if (m_Panel == null)
            return;

        Vector2 size = m_Panel.sizeDelta;
        size.x = m_Sizes[index];
        m_Panel.sizeDelta = size;

        UpdateButtons();
    }

    void UpdateButtons()
    {
        if(m_ButtonShrink)
        {
            bool isVisible = (m_SizeIndex > 0) && !m_IsMinimized;
            m_ButtonShrink.gameObject.SetActive(isVisible);
        }

        if (m_ButtonMinimize)
        {
            bool isVisible = (m_SizeIndex > 0) || m_IsMinimized;
            m_ButtonMinimize.gameObject.SetActive(isVisible);
        }

        if (m_ButtonExpand)
        {
            bool isVisible = (m_SizeIndex < (m_Sizes.Length - 1)) && !m_IsMinimized;
            m_ButtonExpand.gameObject.SetActive(isVisible);
        }
    }
    #endregion

    #region UI Callbacks
    public void ExpandPanel()
    {
        if (m_IsMinimized)
            return;

        int size = Mathf.Min(m_SizeIndex + 1, m_Sizes.Length - 1);
        Resize(size);
    }

    public void ShrinkPanel()
    {
        if (m_IsMinimized)
            return;

        int size = Mathf.Max(m_SizeIndex - 1, 0);
        Resize(size);
    }

    public void MinimizePanel()
    {
        Minimize(!m_IsMinimized);
    }
    #endregion
}
