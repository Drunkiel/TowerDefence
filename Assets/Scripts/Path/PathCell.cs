using UnityEngine;

public class PathCell : MonoBehaviour
{
    public PathType pathType;
    public Rotation rotation;

    private void Start()
    {
        DefineRotation();
    }

    private void DefineRotation()
    {
        switch(Mathf.RoundToInt(transform.localEulerAngles.y))
        {
            case -90:
                rotation = Rotation.East;
                break;

            case 0:
                rotation = Rotation.North;
                break;

            case 90:
                rotation = Rotation.West;
                break;

            case 180:
                rotation = Rotation.South;
                break;
        }
    }
}
