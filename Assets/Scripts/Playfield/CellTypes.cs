using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface CellType
{
    bool IsPassable();
    Sprite GetSprite();
}

public class CellTypeSand : CellType
{
    static Sprite sprite;

    public bool IsPassable() { return true; }
    public Sprite GetSprite()
    {
        if (!sprite)
        {
            sprite = Resources.Load<Sprite>("Sprites/CellTypes/SandTexture");
        }

        return sprite;
    }
}

public class CellTypeRock : CellType
{
    static Sprite sprite;

    public bool IsPassable() { return false; }
    public Sprite GetSprite()
    {
        if (!sprite)
        {
            sprite = Resources.Load<Sprite>("Sprites/CellTypes/StoneTexture");
        }

        return sprite;
    }
}