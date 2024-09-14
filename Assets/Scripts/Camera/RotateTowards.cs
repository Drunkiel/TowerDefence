using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    private void FixedUpdate()
    {
        RotateTowardsCamera();
    }

    private void RotateTowardsCamera()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;

        Quaternion rotationToCamera = Quaternion.LookRotation(-directionToCamera);
        transform.rotation = rotationToCamera;
    }
}