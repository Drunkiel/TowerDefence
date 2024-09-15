using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public bool isStopped;
    public EntityStatistics _statistics;

    public Vector2 movement;
    public bool isMoving;

    [SerializeField] private Rigidbody rgBody;

    [SerializeField] private int pathIndex;
    [SerializeField] private Vector3 positionToGo;

    private bool isNewPositionFound;

    private void Start()
    {
        SetNewPosition();
    }

    private void Update()
    {
        isMoving = movement.magnitude > 0.01f;

        if (isStopped)
            return;

        if (Vector3.Distance(transform.position, positionToGo) < 0.201f)
        {
            if (!isNewPositionFound)
            {
                StartCoroutine(PauseBeforeNewPosition());
                isNewPositionFound = true;
            }
            else
                movement = Vector2.zero;
        }
        else
            GoTo(positionToGo);

        //Controls max speed
        if (rgBody.velocity.magnitude > _statistics.maxSpeed)
            rgBody.velocity = Vector3.ClampMagnitude(rgBody.velocity, _statistics.maxSpeed);
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -10f)
        {
            print($"Enemy fall off the map {transform.name} on position: {transform.position}");
            Destroy(gameObject);
        }

        if (isStopped)
        {
            rgBody.velocity = Vector2.zero;
            return;
        }

        Vector3 move = new Vector3(movement.x, 0, movement.y).normalized;
        rgBody.AddForce(move * (_statistics.speedForce * rgBody.mass), ForceMode.Force);
    }

    private void SetNewPosition()
    {
        pathIndex += 1;
        if (PathController.instance.pathCells.Count <= pathIndex)
        {
            isStopped = true;
            GameController.instance._statistics.TakeDamage(_statistics.health, () => 
            {
                _statistics.TakeDamage(_statistics.health, () => 
                {
                    Destroy(gameObject);
                });
                print("You lost");
            });
            return;
        }

        positionToGo = PathController.instance.pathCells[pathIndex].transform.position;
    }

    private IEnumerator PauseBeforeNewPosition()
    {
        yield return new WaitForSeconds(0);
        isNewPositionFound = false;
        SetNewPosition();
    }

    public void GoTo(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        movement = new Vector2(
            direction.x * _statistics.speedForce,
            direction.z * _statistics.speedForce
        );

        //Flipping Enemy to direction they are going
        // if (movement.x < 0 && !isFlipped)
        // {
        //     transform.GetChild(0).localScale = new(-1, 1, 1);
        //     isFlipped = true;
        // }
        // else if (movement.x > 0 && isFlipped)
        // {
        //     transform.GetChild(0).localScale = new(1, 1, 1);
        //     isFlipped = false;
        // }
    }
}