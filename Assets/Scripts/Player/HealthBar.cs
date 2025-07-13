using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    public Image healthBarFill;
    public Gradient colorGradient;

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }
    
    public void SetHealth(float health)
    {
        currentHealth = health;
        float targetFillAmount = currentHealth / maxHealth;
        healthBarFill.DOFillAmount(targetFillAmount, 0.2f);
        healthBarFill.color = colorGradient.Evaluate(targetFillAmount);
        healthBarFill.DOColor(colorGradient.Evaluate(targetFillAmount), 0.2f);
    }
}
