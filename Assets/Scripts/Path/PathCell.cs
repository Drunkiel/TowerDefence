using System.Collections.Generic;
using UnityEngine;

public class PathCell : MonoBehaviour
{
    public PathType pathType;
    public List<Rotation> rotations = new();

    private void Start()
    {
        DefineRotation();
    }

    private void DefineRotation()
    {
        if (rotations.Count < 1 && rotations.Count > 2)
            return;

        switch(Mathf.RoundToInt(transform.localEulerAngles.y))
        {
            case -90:
                rotations[0] = Rotation.East;
                if (rotations.Count == 2)
                    rotations[1] = Rotation.North;
                break;

            case 0:
                rotations[0] = Rotation.North;
                if (rotations.Count == 2)
                    rotations[1] = Rotation.West;
                break;

            case 90:
                rotations[0] = Rotation.West;
                if (rotations.Count == 2)
                    rotations[1] = Rotation.South;
                break;

            case 180:
                rotations[0] = Rotation.South;
                if (rotations.Count == 2)
                    rotations[1] = Rotation.East;
                break;
        }
    }
}
