using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameEditor : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform m_Panel = null;

    [SerializeField] private Button m_ButtonShrink = null;
    [SerializeField] private Button m_ButtonMinimize = null;
    [SerializeField] private Button m_ButtonExpand = null;

    [SerializeField] private int m_InitialSize = 1;
    [SerializeField] private int[] m_Sizes = null;

    private int m_SizeIndex = 0;
    private bool m_IsMinimized = false;

    void Start ()
    {
        m_IsMinimized = true;
        m_SizeIndex = m_InitialSize;
        ResizePanel(0);
	}
	
    void Resize(int index)
    {
        Debug.Assert((index >= 0) && (index < m_Sizes.Length), "Invalid size index");
        m_SizeIndex = index;

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
        m_IsMinimized = !m_IsMinimized;

        if(m_IsMinimized)
            ResizePanel(0);
        else
            ResizePanel(m_SizeIndex);
    }
}
