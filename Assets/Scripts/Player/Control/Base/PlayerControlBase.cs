using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerControlBase : MonoBehaviour
{
    private PlayerEvents m_PlayerEvents = new PlayerEvents();

    public PlayerEvents PlayerEvents
    { get { return m_PlayerEvents; } }
    
    protected void Update ()
    {
        m_PlayerEvents.Update(Time.deltaTime);
        UpdateControl();
    }

    protected abstract void UpdateControl();
}
