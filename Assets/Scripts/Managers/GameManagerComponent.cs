using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComponent : SingletonMonoBehaviour<GameManagerComponent>
{
	void Start ()
    {
        Init();
    }

    public void Init()
    {
        UpdateCursor();
    }

    void Update ()
    {
        UpdateCursor();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ShowCursor();
        else
            UpdateCursor();
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
            UpdateCursor();
        else
            ShowCursor();
    }

    void UpdateCursor()
    {/*
        if (LevelManager.Instance.GetLevelComponent() == null)
            ShowCursor();
        else
            HideCursor();*/
    }

    void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
