using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlEventsManager
{
    #region Members
    private List<ControllerEventBase>  m_Controllers = new List<ControllerEventBase>();
    private Dictionary<int, object>    m_InputEvents = new Dictionary<int, object>();
    #endregion

    #region Update
    public void Update(float dt)
    {
        ClearEvents();
        UpdateControllers(dt);
    }
    #endregion

    #region Controller
    protected void AddController(ControllerEventBase _controller)
    {
        _controller.SetManager(this);
        m_Controllers.Add(_controller);
    }

    protected void UpdateControllers(float dt)
    {
        foreach (ControllerEventBase controller in m_Controllers)
        {
            controller.Update(dt);

            Dictionary<int, object> events = controller.InputEvents;
            if (SetEvents(events) && controller.Exclusive)
                break;
        }
    }
    #endregion

    #region Events
    private void ClearEvents()
    {
        m_InputEvents.Clear();
    }

    public bool HasEvents
    {
        get { return (m_InputEvents.Count > 0); }
    }

    public virtual bool IsValidEvent<TEvent>(TEvent _event)
    {
        //return typeof(TEvent).IsEnum || typeof(TEvent).IsPrimitive;
        return true;
    }

    protected void SetEvent<TEvent>(TEvent _event)
    {
        SetEvent(_event, null);
    }

    protected void SetEvent<TEvent>(TEvent _event, object _value)
    {
        Debug.Assert(IsValidEvent(_event), "Set invalid input event");

        if (IsValidEvent(_event))
        {
            int index = Convert.ToInt32(_event);
            m_InputEvents[index] = _value;
        }
    }

    public object GetEvent<TEvent>(TEvent _event)
    {
        Debug.Assert(IsValidEvent(_event), "Get invalid input event");

        object result = null;

        if (IsValidEvent(_event))
        {
            int index = Convert.ToInt32(_event);
            m_InputEvents.TryGetValue(index, out result);
        }

        return result;
    }

    public bool HasEvent<TEvent>(TEvent _event)
    {
        Debug.Assert(IsValidEvent(_event), "Get invalid input event");

        bool result = false;

        if (IsValidEvent(_event))
        {
            int index = Convert.ToInt32(_event);
            result = m_InputEvents.ContainsKey(index);
        }

        return result;
    }

    private bool SetEvents(Dictionary<int, object> _events)
    {
        if (_events.Count == 0)
            return false;

        foreach (KeyValuePair<int, object> inputEvent in _events)
            SetEvent(inputEvent.Key, inputEvent.Value);

        return true;
    }
    #endregion
}
