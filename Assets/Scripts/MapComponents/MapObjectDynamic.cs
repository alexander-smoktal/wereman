using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapObjectDynamic : MapObjectComponent
{
	void Update ()
    {
        AttachCells(hexGrid.GetCells(collider));

        DrawDebugInfo();
    }
}