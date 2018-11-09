using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : Singleton<ScenesManager>
{
    public enum SceneId
    {
        Invalid = -1,
        StartScreen = 0,
        
        MainScene,

        Count
    }

    public void LoadScene(SceneId _id)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)_id);
    }
}
