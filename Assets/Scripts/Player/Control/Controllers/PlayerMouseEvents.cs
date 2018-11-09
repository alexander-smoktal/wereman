using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseEvents : ControllerEventBase
{
    #region Members
    /*
    const float cfZero = 0.0f;
    const float cfOne = 1.0f;
    */
    #endregion

    #region Constructors
    public PlayerMouseEvents()
    {
    }

    public PlayerMouseEvents(bool _isExclusive) : base(_isExclusive)
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
    {/*
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if ((x != cfZero) || (y != cfZero))
        {
            Vector2 offset = new Vector2(y, x);
            SetEvent(PlayerEvents.InputEvents.MoveTo, offset);
        }*/
    }

    private void UpdateActions()
    {/*
        if (Input.GetMouseButtonDown(0))
            SetEvent(PlayerEvents.InputEvents.Attack);*/
    }
    #endregion
}
