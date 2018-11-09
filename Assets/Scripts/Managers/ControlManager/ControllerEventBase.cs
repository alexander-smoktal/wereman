using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerEventBase
{
    #region Constants
    private const string c_AssertPrefix = "[ControlEventBase] ";
    #endregion

    #region Members
    private ControlEventsManager    m_EventsManager = null;
    private Dictionary<int, object> m_InputEvents   = new Dictionary<int, object>();
    private bool                    m_Exlusive      = true;
    #endregion

    #region Properties
    public bool Exclusive
    { get { return m_Exlusive; } }

    public ControlEventsManager Manager
    { get { return m_EventsManager; } }

    public Dictionary<int, object> InputEvents
    { get { return m_InputEvents; } }
    #endregion

    #region Constructors
    public ControllerEventBase()
    {
    }

    public ControllerEventBase(bool _isExclusive)
    {
        m_Exlusive = _isExclusive;
    }
    #endregion

    #region Events
    public void SetManager(ControlEventsManager _manager)
    {
        Debug.Assert(_manager != null, c_AssertPrefix + "Invalid manager");
        Debug.Assert((m_EventsManager == null) || (m_EventsManager == _manager), c_AssertPrefix + "Can't change manager");

        m_EventsManager = _manager;
    }

    private void ClearEvents()
    {
        m_InputEvents.Clear();
    }

    private bool IsValidEvent<TEvent>(TEvent _event)
    {
        return (m_EventsManager != null) && m_EventsManager.IsValidEvent(_event);
    }

    protected void SetEvent<TEvent>(TEvent _event)
    {
        SetEvent(_event, null);
    }

    protected void SetEvent<TEvent>(TEvent _event, object _value)
    {
        Debug.Assert(IsValidEvent(_event), c_AssertPrefix + "Set invalid input event");

        if (IsValidEvent(_event))
        {
            int index = Convert.ToInt32(_event);
            m_InputEvents[index] = _value;
        }
    }
    #endregion

    #region Update
    public void Update(float dt)
    {
        ClearEvents();
        UpdateInputs(dt);
    }

    protected abstract void UpdateInputs(float dt);
    #endregion
}
