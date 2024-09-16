using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    
    public Transform target;           // Target around which the camera will rotate
    public float rotationSpeed = 100f; // Speed of camera rotation
    public float zoomSpeed = 10f;      // Speed of zoom in/out
    public float minZoom = 20f;        // Minimum Field of View (zoom in limit)
    public float maxZoom = 60f;        // Maximum Field of View (zoom out limit)
    public float distanceFromTarget = 10f; // Initial distance from the target

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    // Limits for the vertical rotation (X axis), preventing the camera from going below the target
    public float minVerticalAngle = -10f; // Min angle, prevent the camera from going below the target
    public float maxVerticalAngle = 80f;  // Max angle, prevents too much upward rotation

    private void Start()
    {
        // Set initial position based on the target's position
        if (target != null)
        {
            transform.position = target.position - transform.forward * distanceFromTarget;
        }
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();
    }

    // Method to handle camera rotation around the target
    private void HandleRotation()
    {
        // Check if the right mouse button is held down for rotation
        if (Input.GetMouseButton(1))
        {
            // Get mouse input for horizontal and vertical movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Update the current rotations based on mouse movement
            currentRotationY += mouseX * rotationSpeed * Time.deltaTime;
            currentRotationX -= mouseY * rotationSpeed * Time.deltaTime;

            // Clamp vertical rotation to avoid flipping the camera over and going below the object
            currentRotationX = Mathf.Clamp(currentRotationX, minVerticalAngle, maxVerticalAngle);

            // Update the camera position based on the new rotation
            if (target != null)
            {
                // Rotate around the target while maintaining distance
                Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
                Vector3 direction = rotation * Vector3.back * distanceFromTarget;
                transform.position = target.position + direction;

                // Always look at the target
                transform.LookAt(target);
            }
        }
    }

    // Method to handle camera zoom
    private void HandleZoom()
    {
        // Get the scroll wheel input (positive for zooming in, negative for zooming out)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Update the distance from the target based on the scroll input
        distanceFromTarget -= scrollInput * zoomSpeed;
        
        // Clamp the distance to avoid zooming too close or too far
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minZoom, maxZoom);

        // Update the camera's position after zooming
        if (target != null)
        {
            Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
            Vector3 direction = rotation * Vector3.back * distanceFromTarget;
            transform.position = target.position + direction;
        }
    }
}
