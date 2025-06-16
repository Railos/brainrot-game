using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; }
    private bool canReceiveDamage = true;
    [SerializeField] private float invincibilityTimeAfterTakingDamage = 0.5f;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    
    public void Damage(float damageAmount)
    {
        if (!canReceiveDamage) return;
        CurrentHealth -= damageAmount;
        canReceiveDamage = false;
        if (CurrentHealth <= 0f)
        {
            Die();
        }
        Invoke(nameof(ResetCanReceiveDamage),invincibilityTimeAfterTakingDamage);
    }

    public void Die()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetCanReceiveDamage()
    {
        canReceiveDamage = true;
    }
    
}
