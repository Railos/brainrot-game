using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    public Action actionAfterDeath;
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
            if (actionAfterDeath != null)
                actionAfterDeath.Invoke();
            else DestroyObjectAction();
        }
    }

    public void Heal(int value)
    {
        health += value;
        health = Mathf.Round(maxHealth);
    }

    public void DestroyObjectAction()
    {
        Destroy(this.gameObject);
    }
}
