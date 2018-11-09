using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    public enum Events
    {
        Invalid = -1,

        UpdateText,

        Count
    }

    private Dictionary<Events, UnityEvent> m_Listeners = new Dictionary<Events, UnityEvent>();

    #region Listeners
    public void RegisterListener(Events _event, UnityAction _listener)
    {
        Debug.Assert((_event > Events.Invalid) && (_event < Events.Count), "Invalid event");

        UnityEvent unityEvent = null;
        if (m_Listeners.TryGetValue(_event, out unityEvent))
        {
            unityEvent.AddListener(_listener);
        }
        else
        {
            unityEvent = new UnityEvent();
            unityEvent.AddListener(_listener);

            m_Listeners.Add(_event, unityEvent);
        }
    }

    public void UnregisterListener(Events _event, UnityAction _listener)
    {
        Debug.Assert((_event > Events.Invalid) && (_event < Events.Count), "Invalid event");

        UnityEvent unityEvent = null;
        if (m_Listeners.TryGetValue(_event, out unityEvent))
            unityEvent.RemoveListener(_listener);
    }

    public void TrigerEvent(Events _event)
    {
        Debug.Assert((_event > Events.Invalid) && (_event < Events.Count), "Invalid event");

        UnityEvent unityEvent = null;
        if (m_Listeners.TryGetValue(_event, out unityEvent))
            unityEvent.Invoke();
    }
    #endregion
}
