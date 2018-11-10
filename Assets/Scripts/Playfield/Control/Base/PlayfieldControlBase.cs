using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayfieldControlBase : MonoBehaviour
{
    private PlayfieldEvents m_PlayfieldEvents = new PlayfieldEvents();

    public PlayfieldEvents PlayfieldEvents
    { get { return m_PlayfieldEvents; } }
    
    protected void Update ()
    {
        m_PlayfieldEvents.Update(Time.deltaTime);
        UpdateControl();
    }

    protected abstract void UpdateControl();
}
