﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
	public static class Geometry {
		public const float outerRadius = 10f;
		public const float innerRadius = outerRadius * 0.866025404f;

		public static Vector3[] corners = {
			new Vector3(0f, 0f, outerRadius),
			new Vector3(innerRadius, 0f, 0.5f * outerRadius),
			new Vector3(innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(0f, 0f, -outerRadius),
			new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
		};
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
