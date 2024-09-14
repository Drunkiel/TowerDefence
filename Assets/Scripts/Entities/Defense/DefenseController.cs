using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    public EntityStatistics _statistics;
    public List<GameObject> detectedEnemies = new();
    private bool attacked;

    private void Update()
    {
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
        detectedEnemies[0].GetComponent<EnemyController>()._statistics.TakeDamage(_statistics.damage);
        attacked = false;
    }

    private void RotateTowardsFirstEnemy()
    {
        if (detectedEnemies.Count > 0)
        {
            GameObject firstEnemy = detectedEnemies[0];
            Vector3 direction = (firstEnemy.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.parent.GetChild(0).rotation = Quaternion.Slerp(transform.parent.GetChild(0).rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
