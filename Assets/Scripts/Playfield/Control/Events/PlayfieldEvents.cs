using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldEvents : ControlEventsManager
{
    public enum InputEvents
    {
        Invalid = -1,
        
        CursorMove,
        LMBPressed,
        Select,
        CursorMoveOverUI,

        Count
    }

    private PlayfieldMouseEvents    m_MouseController    = new PlayfieldMouseEvents();
    private PlayfieldKeyboardEvents m_KeyboardController = new PlayfieldKeyboardEvents();
    private PlayfieldGamepadEvents  m_GamepadController  = new PlayfieldGamepadEvents();

    public PlayfieldEvents()
    {
        AddController(m_MouseController);
        AddController(m_KeyboardController);
        AddController(m_GamepadController);
    }

    private bool IsInRange(int _event)
    {
        return ((int)InputEvents.Invalid < _event) && (_event < (int)InputEvents.Count);
    }

    public override bool IsValidEvent<TEvent>(TEvent _event)
    {
        return base.IsValidEvent(_event) && IsInRange(Convert.ToInt32(_event));
    }

    public void SetPlayfieldComponent(HexGrid _playfieldComponent)
    {
        m_MouseController.SetPlayfieldComponent(_playfieldComponent);
        m_KeyboardController.SetPlayfieldComponent(_playfieldComponent);
        m_GamepadController.SetPlayfieldComponent(_playfieldComponent);
    }
}
