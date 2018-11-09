using UnityEngine;
using System.Collections;

public class PopupMenu : MenuBase
{
    public delegate void OnClick();

    private struct OnClickCallback
    {
        public OnClick m_OnClick;
    }

    public static void OnPressEmpty()
    { }

    private OnClickCallback[] m_Callback = null;
	[SerializeField] private int m_CallbacksCount = 0;

    public PopupMenu()
    {
		m_Callback = new OnClickCallback[m_CallbacksCount];
        ClearCallbacks();
    }

    void Awake()
    {
        m_Callback = new OnClickCallback[m_CallbacksCount];
        ClearCallbacks();
    }

    protected override void OnShow()
    {
        Debug.Assert(CheckCallback(), "uninitialized callback");
    }

    private void ClearCallbacks()
    {
        if (m_Callback == null)
            return;

        for (int i = 0; i < m_Callback.Length; ++i)
            m_Callback[i].m_OnClick = null;
    }
    
    private bool CheckCallback()
    {
        if (m_Callback == null)
            return false;

        bool result = (m_Callback.Length > 0);

        for (int i = 0; i < m_Callback.Length; ++i)
        {
            if (m_Callback[i].m_OnClick == null)
            {
                result = false;
                m_Callback[i].m_OnClick = PopupMenu.OnPressEmpty;
            }
        }

        return result;
    }

    public void SetCallback(int _index, OnClick _callback)
    {
        if (m_Callback == null)
            return;

        Debug.Assert(_index < m_Callback.Length, "Bad callback index");

        if (_index < m_Callback.Length)
            m_Callback[_index].m_OnClick = _callback;
    }

    public void PopUpButtonPress(int _index)
    {
        if (m_Callback == null)
            return;

        Debug.Assert(_index < m_Callback.Length, "Bad callback index");

        if (_index < m_Callback.Length)
            m_Callback[_index].m_OnClick();

        ClearCallbacks();

        Back();
    }
}
