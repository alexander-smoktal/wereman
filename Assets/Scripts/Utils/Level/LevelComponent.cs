using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{

    void Awake()
    {
        LevelManager.Instance.RegisterLevelComponent(this);
    }

    private void OnDestroy()
    {
        LevelManager.Instance.UnregisterLevelComponent();
    }

    void Start()
    {

    }
    
    void Update()
    {

    }
}
