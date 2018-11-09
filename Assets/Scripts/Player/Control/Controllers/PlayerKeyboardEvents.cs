using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardEvents : ControllerEventBase
{
    #region Constructors
    public PlayerKeyboardEvents()
    {
    }

    public PlayerKeyboardEvents(bool _isExclusive) : base(_isExclusive)
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
        UpdateLocomotion();
    }

    private void UpdateLocomotion()
    {
    }

    private void UpdateActions()
    {

    }
    #endregion
}
