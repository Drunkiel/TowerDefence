using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EntityStatistics
{
    [Header("Health / Mana")]
    public int health;
    public int maxHealth;

    [Header("Damage")]
    public int damage;
    public float attackSpeed;

    [Header("Movement")]
    public float speedForce;
    public float maxSpeed = 1.2f;

    [SerializeField] private Slider healthSlider;

    public void TakeDamage(int amount, Action deathAction)
    {
        int damageToDeal = amount;

        health -= damageToDeal;
        healthSlider.value = (float)health / maxHealth;
        if (health <= 0)
        {
            health = 0;
            deathAction.Invoke();
        }
    }
}