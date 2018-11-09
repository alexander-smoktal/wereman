using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour
{
    [SerializeField] private bool m_OpenOnStart = false;

    MenuBase m_PrevMenu = null;

    protected MenuBase PrevMenu
    { get { return m_PrevMenu; } }
    
    void Start ()
    {
        if (m_OpenOnStart)
            Open(m_PrevMenu);
        else
            Hide();
    }

    protected virtual void OnShow()
    { }

    protected virtual void OnHide()
    { }

    protected void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    protected void Hide()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    public void Open(MenuBase _prevMenu = null)
    {
        Debug.Assert(m_PrevMenu == null, "The menu (" + gameObject.name + ") is already opened");

        if (m_PrevMenu != null)
            return;

        if(_prevMenu)
            _prevMenu.Hide();

        m_PrevMenu = _prevMenu;

        Show();
    }

    public void OpenMenu(MenuBase _menu)
    {
        Debug.Assert(_menu != null, "Invalid menu");

        if (_menu == null)
            return;

        _menu.Open(this);
    }

    public void Back()
    {
        Hide();

        MenuBase _prevMenu = m_PrevMenu;
        m_PrevMenu = null;

        if(_prevMenu)
            _prevMenu.Show();
    }

    public void OnButtonMenuOpen(MenuBase _menu)
    {
        if (_menu != null)
            _menu.Open(this);
    }
}
