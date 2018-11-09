using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private LevelComponent m_LevelComponent = null;

    public void RegisterLevelComponent(LevelComponent levelComponent)
    {
        Debug.Assert(levelComponent != null, "Invalid level component");
        Debug.Assert(m_LevelComponent == null, "Level component is already registered");
        m_LevelComponent = levelComponent;
    }

    public void UnregisterLevelComponent()
    {
        Debug.Assert(m_LevelComponent != null, "Level component is not registered");
        m_LevelComponent = null;
    }

    public LevelComponent GetLevelComponent()
    {
        return m_LevelComponent;
    }
}
