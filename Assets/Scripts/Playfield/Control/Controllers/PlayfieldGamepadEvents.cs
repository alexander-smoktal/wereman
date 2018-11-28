using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldGamepadEvents : ControllerEventBase
{
    #region Constants
    private const float m_UpdatePeriod = 0.1f;
    #endregion

    #region Members
    private HexGrid m_PlayfieldComponent;

    private float m_MovementDelay = 0;
    #endregion

    #region Constructors
    public PlayfieldGamepadEvents()
    {
    }

    public PlayfieldGamepadEvents(bool _isExclusive) : base(_isExclusive)
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

        if(UpdateMovementDelay())
            UpdateMovement();

        UpdateActions();
    }

    private int GetAxisInput(string name)
    {
        float axis = Input.GetAxis(name);
        int i = 0;
        
            if (axis == 0)
                i = 0;
            else if (axis > 0)
                i = 1;
            else
                i = -1;

        return i;
    }

    private bool UpdateMovementDelay()
    {
        if(m_MovementDelay > 0)
        {
            m_MovementDelay -= Time.deltaTime;
            return false;
        }

        return true;
    }

    private void SetMovementDelay()
    {
        m_MovementDelay = m_UpdatePeriod;
    }

    private void UpdateMovement()
    {
        int dx = GetAxisInput("Pad Horizontal");
        int dy = GetAxisInput("Pad Vertical");

        if ((dx == 0) && (dy == 0))
            return;

        SetMovementDelay();

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
