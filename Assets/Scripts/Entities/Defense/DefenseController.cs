using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    public EntityStatistics _statistics;
    public List<GameObject> detectedEnemies = new();
    public GameObject currentTarget;
    private bool attacked;
    private Animator anim;

    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();    
    }

    private void Update()
    {
        if (currentTarget == null && detectedEnemies.Count > 0)
            currentTarget = detectedEnemies[0];

        RotateTowardsFirstEnemy();
        Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if object is not in the list
        if (other.CompareTag("Enemy") && !detectedEnemies.Contains(other.gameObject))
            detectedEnemies.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        // Delete object from the list
        if (other.CompareTag("Enemy") && detectedEnemies.Contains(other.gameObject))
            detectedEnemies.Remove(other.gameObject);
    }

    private void Attack()
    {
        if (detectedEnemies.Count > 0 && !attacked)
        {
            StartCoroutine(WaitAndAttack());
            attacked = true;
        }
    }

    private IEnumerator WaitAndAttack()
    {
        yield return new WaitForSeconds(_statistics.attackSpeed);

        if (detectedEnemies.Count <= 0)
        {
            attacked = false;
            yield break;
        }

        anim.Play("Attack");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        detectedEnemies[0].GetComponent<EnemyController>()._statistics.TakeDamage(_statistics.damage, () => 
        {
            //Destroy object
            Destroy(detectedEnemies[0]);
            detectedEnemies.RemoveAt(0);

            //Give reward
        });
        attacked = false;
    }

    private void RotateTowardsFirstEnemy()
    {
        if (detectedEnemies.Count > 0)
        {
            if (currentTarget == null)
            {
                detectedEnemies.RemoveAt(0);
                return;
            }

            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.parent.GetChild(0).rotation = Quaternion.Slerp(transform.parent.GetChild(0).rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
