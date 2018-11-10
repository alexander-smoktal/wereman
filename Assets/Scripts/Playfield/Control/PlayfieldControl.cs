using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldControl : PlayfieldControlBase
{
    #region Members
    private bool m_IsPaused = false;
    private bool m_PrevPaused = false;

    private HexGrid m_PlayfieldComponent = null;

    private Vector2Int m_CurrentCell = new Vector2Int(-1, -1);
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        m_PlayfieldComponent = GetComponent<HexGrid>();
        Debug.Assert(m_PlayfieldComponent != null, "Invalid HexGrid Component");

        PlayfieldEvents.SetPlayfieldComponent(m_PlayfieldComponent);
    }

    void Start ()
    {

    }

    protected void FixedUpdate()
    {
        if (IsPaused())
            return;


    }
    #endregion

    #region Pause
    public void SetPause(bool _value)
    {
        m_IsPaused = _value;
    }

    public bool IsPaused()
    {
        return m_IsPaused || m_PrevPaused;
    }

    private bool UpdatePause()
    {
        bool paused = IsPaused();
        m_PrevPaused = m_IsPaused;

        return paused;
    }
    #endregion

    #region Controls
    protected override void UpdateControl()
    {
        if (UpdatePause())
            return;

        if (PlayfieldEvents.HasEvents)
        {
            UpdateMovement();
            UpdateActions();
        }
    }

    private void UpdateMovement()
    {
        UpdateLocomotion();
    }

    private bool IsValidCell(Vector2Int _index)
    {
        return (_index.x >= 0) && (_index.y >= 0);
    }

    private void UpdateLocomotion()
    {
        if (!PlayfieldEvents.HasEvent(PlayfieldEvents.InputEvents.CursorMove))
            return;

        m_CurrentCell = (Vector2Int)PlayfieldEvents.GetEvent(PlayfieldEvents.InputEvents.CursorMove);

        if (IsValidCell(m_CurrentCell))
            m_PlayfieldComponent.HiglightCell(m_CurrentCell.x, m_CurrentCell.y);
        else
            m_PlayfieldComponent.RemoveHiglighting();
    }

    private void UpdateActions()
    {
        if (!PlayfieldEvents.HasEvent(PlayfieldEvents.InputEvents.Select))
            return;

        if (IsValidCell(m_CurrentCell))
            m_PlayfieldComponent.SelectCell(m_CurrentCell.x, m_CurrentCell.y);
        else
            m_PlayfieldComponent.RemoveSelection();
    }
    #endregion
}
