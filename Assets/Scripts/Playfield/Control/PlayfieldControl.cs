using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldControl : PlayfieldControlBase
{
    #region Members
    private bool m_IsPaused = false;
    private bool m_PrevPaused = false;

    private bool m_IsCursorOverUI = false;

    private HexGrid m_PlayfieldComponent = null;

    private Vector2Int m_CurrentCell = new Vector2Int(-1, -1);

    private bool                    m_Edit = false;
    private InGameEditor.Properties m_EditorProperties;
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

    #region Editor Properties
    public void SetEditMode(bool edit)
    {
        m_Edit = edit;
    }

    public void SetEditorProperties(InGameEditor.Properties editorProperties)
    {
        m_EditorProperties = editorProperties;
    }

    public void SaveMap()
    {
        m_PlayfieldComponent.SaveMap();
    }
    #endregion

    #region Editor Controls
    private void EditCells()
    {
        EditorUpdateSelection();
        EditorUpdateCell();
    }

    private void EditorUpdateSelection()
    {
        if (!PlayfieldEvents.HasEvent(PlayfieldEvents.InputEvents.CursorMove))
            return;

        m_CurrentCell = (Vector2Int)PlayfieldEvents.GetEvent(PlayfieldEvents.InputEvents.CursorMove);

        if (IsValidCell(m_CurrentCell))
            m_PlayfieldComponent.SetEditorSelection(m_CurrentCell.x, m_CurrentCell.y);
    }

    private void EditorUpdateCell()
    {
        if (!PlayfieldEvents.HasEvent(PlayfieldEvents.InputEvents.Select))
            return;

        if (IsValidCell(m_CurrentCell))
            m_PlayfieldComponent.SetCellProperties(m_CurrentCell.x, m_CurrentCell.y, m_EditorProperties);
    }
    #endregion

    #region Controls
    protected override void UpdateControl()
    {
        if (UpdatePause())
            return;

        if (PlayfieldEvents.HasEvents)
        {
            if (m_Edit)
            {
                EditCells();
            }
            else
            {
                UpdateMovement();
                UpdateActions();
                UpdateUI();
            }
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

    private void UpdateUI()
    {
        bool isOverUI = PlayfieldEvents.HasEvent(PlayfieldEvents.InputEvents.CursorMoveOverUI);

        if (isOverUI && !m_IsCursorOverUI)
        {
            m_PlayfieldComponent.RemoveHiglighting();
            m_PlayfieldComponent.OverUI();
        }

        m_IsCursorOverUI = isOverUI;
    }
    #endregion
}
