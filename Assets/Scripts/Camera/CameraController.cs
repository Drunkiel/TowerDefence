using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public bool isStopped;
    
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

    [SerializeField] private Vector2 movement;
    private Vector2 newVelocityXZ;
    public bool isMoving;

    [SerializeField] private Rigidbody rgBody;
    public float movementDamping = 5f;   // Damping for velocity reduction when not moving

    private void Start()
    {
        // Set initial position based on the target's position
        if (target != null)
        {
            transform.position = target.position - transform.forward * distanceFromTarget;
            virtualCamera.transform.LookAt(target);
        }
    }

    private void Update()
    {
        HandleRotation();
        HandleZoom();

        // Movement, jump and animations control
        isMoving = movement.magnitude > 0.1f;

        if (isStopped)
            return;

        // If not moving, dampen the velocity
        if (!isMoving)
        {
            // Smoothly reduce velocity to zero when no movement input is provided
            rgBody.velocity = Vector3.Lerp(rgBody.velocity, Vector3.zero, Time.deltaTime * movementDamping);
        }

        // Clamping movement speed
        newVelocityXZ = new Vector2(rgBody.velocity.x, rgBody.velocity.z);

        if (newVelocityXZ.magnitude > 1.5f)
            newVelocityXZ = Vector2.ClampMagnitude(newVelocityXZ, 1.5f);

        rgBody.velocity = new Vector3(newVelocityXZ.x, rgBody.velocity.y, newVelocityXZ.y);
    }

    private void FixedUpdate()
    {
        // Calculate movement direction based on camera's forward and right vectors
        if (target != null)
        {
            Vector3 forward = target.forward;
            Vector3 right = target.right;

            // Normalize the movement vector and apply it based on camera's orientation
            Vector3 moveDirection = (movement.x * right + movement.y * forward).normalized;

            // Apply the calculated movement direction to the Rigidbody
            rgBody.AddForce(moveDirection * 10, ForceMode.Acceleration);
        }
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();

        if (isStopped)
            return;

        movement = new Vector2(inputValue.x, inputValue.y);
    }

    // Handle rotation and rotate target to match the camera
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
                virtualCamera.transform.position = target.position + direction;

                // Always look at the target
                virtualCamera.transform.LookAt(target);

                // Rotate the target to follow the camera's horizontal movement (Y-axis only)
                target.rotation = Quaternion.Euler(0, currentRotationY, 0);
            }
        }
    }

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
            virtualCamera.transform.position = target.position + direction;
        }
    }
}
