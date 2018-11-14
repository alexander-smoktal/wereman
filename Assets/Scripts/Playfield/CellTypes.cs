using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellType
{
    [Flags]
    public enum Type
    {
        None = 0,
        Sand = 1,
        Grass = 1 << 1,
        Dirt = 1 << 2,
        Stone = 1 << 4,
    }

    Type type;

    public CellType(Type type)
    {
        this.type = type;
    }

    public Type Get()
    {
        return type;
    }

    public bool IsPassable()
    {
        return (type & Type.Stone) == Type.None;
    }
}
