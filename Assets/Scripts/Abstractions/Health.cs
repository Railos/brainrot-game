using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
