using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : ControlEventsManager
{
    public enum InputEvents
    {
        Invalid = -1,
        
        MoveTo,
        Attack,

        Count
    }

    public PlayerEvents()
    {
        AddController(new PlayerMouseEvents(false));
        AddController(new PlayerKeyboardEvents());
        AddController(new PlayerGamepadEvents());
    }

    private bool IsInRange(int _event)
    {
        return ((int)InputEvents.Invalid < _event) && (_event < (int)InputEvents.Count);
    }

    public override bool IsValidEvent<TEvent>(TEvent _event)
    {
        return base.IsValidEvent(_event) && IsInRange(Convert.ToInt32(_event));
    }
}
