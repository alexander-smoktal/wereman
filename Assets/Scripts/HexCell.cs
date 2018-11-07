using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
    static public Color clickColor = Color.red;
    static public Color defaultColor = Color.white;
    static public Color hoverColor = Color.magenta;
    static public Color neighbourColor = Color.green;

    public HexCoordinates coordinates;
    public Color color;
    // Original cell color. Should be changed to cell type color
    public Color originalColor = defaultColor;
    public int row;
    public int column;

    public static class Geometry {
		public const float outerRadius = 10f;
		public const float innerRadius = outerRadius * 0.866025404f;

		public static Vector3[] corners = {
			new Vector3(0f, 0f, outerRadius),
			new Vector3(innerRadius, 0f, 0.5f * outerRadius),
			new Vector3(innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(0f, 0f, -outerRadius),
			new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
			new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius)
        };
	}

    public bool IsPointInside(Vector3 point)
    {
        Vector3 position = transform.position;
        return Math.Abs(position.x - point.x) < Geometry.innerRadius
            && Math.Abs(position.y - point.y) < Geometry.innerRadius;
    }
}
