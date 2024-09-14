using System.Collections.Generic;
using UnityEngine;

public enum PathType
{
    Start,
    Straight,
    Corner,
    Cross,
    End,
}

public enum Rotation
{
    East,
    North,
    West,
    South,
}

public class PathController : MonoBehaviour
{
    public static PathController instance;
    public List<PathCell> pathCells = new();

    void Awake()
    {
        instance = this;
    }
}
