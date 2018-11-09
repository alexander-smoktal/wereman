using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGamepadEvents : ControllerEventBase
{
    #region Constructors
    public PlayerGamepadEvents()
    {
    }

    public PlayerGamepadEvents(bool _isExclusive) : base(_isExclusive)
    {
    }
    #endregion

    #region Update Inputs
    protected override void UpdateInputs(float dt)
    {
        UpdateMovement();
        UpdateActions();
    }

    private void UpdateMovement()
    {

    }

    private void UpdateActions()
    {

    }
    #endregion
}
