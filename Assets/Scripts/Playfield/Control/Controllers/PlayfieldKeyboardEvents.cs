using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldKeyboardEvents : ControllerEventBase
{
    #region Members
    private HexGrid m_PlayfieldComponent;
    #endregion

    #region Constructors
    public PlayfieldKeyboardEvents()
    {
    }

    public PlayfieldKeyboardEvents(bool _isExclusive) : base(_isExclusive)
    {
    }

    public void SetPlayfieldComponent(HexGrid _playfieldComponent)
    {
        m_PlayfieldComponent = _playfieldComponent;
    }
    #endregion

    #region Update Inputs
    protected override void UpdateInputs(float dt)
    {
        Debug.Assert(m_PlayfieldComponent != null, "Invalid palyfield component");

        UpdateMovement();
        UpdateActions();
    }

    private int GetAxisInput(string name)
    {
        int i = 0;

        if (Input.GetButtonDown(name))
        {
            if (Input.GetAxis(name) > 0)
                i = 1;
            else
                i = -1;
        }

        return i;
    }

    private void UpdateMovement()
    {
        int dx = GetAxisInput("Horizontal");
        int dy = GetAxisInput("Vertical");

        if ((dx == 0) && (dy == 0))
            return;

        var cellIndex = m_PlayfieldComponent.GetHiglightedCell();

        cellIndex.x += dx;
        cellIndex.y += dy;

        cellIndex.x = Mathf.Clamp(cellIndex.x, 0, m_PlayfieldComponent.width-1);
        cellIndex.y = Mathf.Clamp(cellIndex.y, 0, m_PlayfieldComponent.height-1);

        SetEvent(PlayfieldEvents.InputEvents.CursorMove, cellIndex);
    }

    private void UpdateActions()
    {
        if (Input.GetButtonDown("Select"))
            SetEvent(PlayfieldEvents.InputEvents.Select);
    }
    #endregion
}
