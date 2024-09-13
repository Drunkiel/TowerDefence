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
    public List<PathCell> pathCells = new();
}
