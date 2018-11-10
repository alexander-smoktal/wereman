﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldEvents : ControlEventsManager
{
    public enum InputEvents
    {
        Invalid = -1,
        
        CursorMove,
        Select,

        Count
    }

    private PlayfieldMouseEvents m_MouseController = new PlayfieldMouseEvents();

    public PlayfieldEvents()
    {
        AddController(m_MouseController);
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
    }
}
