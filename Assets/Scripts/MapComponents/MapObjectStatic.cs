using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapObjectStatic : MapObjectComponent
{
    void Start ()
    {
        AttachCells(hexGrid.GetCells(collider));
	}
	
	// Update is called once per frame
	void Update ()
    {
        DrawDebugInfo();
    }
}
