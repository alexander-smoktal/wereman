using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpriteRenderer : MonoBehaviour {
    public void SetSprite(Sprite sprite, int order)
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = order;
    }
}
