using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
	void Start ()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.SceneId.MainScene);
	}
	
	void Update ()
    {
		
	}
}
