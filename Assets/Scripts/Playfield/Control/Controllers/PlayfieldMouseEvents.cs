using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldMouseEvents : ControllerEventBase
{
    #region Members
    private Vector3 m_PrevMousePosition = Vector3.forward; // Set invalid mouse poition

    private HexGrid m_PlayfieldComponent;
    #endregion

    #region Constructors
    public PlayfieldMouseEvents()
    {
    }

    public PlayfieldMouseEvents(bool _isExclusive) : base(_isExclusive)
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

        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (!isOverUI)
        {
            UpdateMovement();
            UpdateActions();
        }
        else
        {
            SetEvent(PlayfieldEvents.InputEvents.CursorOverUI);
        }
    }

    private void UpdateMovement()
    {
        var mousePosition = Input.mousePosition;

        // Check if mouse has moved
        if (m_PrevMousePosition == mousePosition)
            return;

        m_PrevMousePosition = mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        var cellIndex = m_PlayfieldComponent.GetCell(ray);

        SetEvent(PlayfieldEvents.InputEvents.CursorMove, cellIndex);
    }

    private void UpdateActions()
    {
        if (Input.GetMouseButtonDown(0))
            SetEvent(PlayfieldEvents.InputEvents.Select);
    }
    #endregion
}
