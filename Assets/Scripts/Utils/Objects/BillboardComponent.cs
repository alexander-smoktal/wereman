using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
        UpdateOrientation();
    }

    void UpdateOrientation()
    {
        Camera camera = Camera.main;
        if (!camera)
            return;

        Vector3 up = Vector3.up;
        Vector3 forward = Vector3.Cross(camera.transform.right, up).normalized;

        transform.forward = forward;
    }
}
