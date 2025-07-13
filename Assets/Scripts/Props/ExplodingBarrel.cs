using UnityEngine;

public class ExplodingBarrel : MonoBehaviour, IDamageable
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 30;
    [SerializeField] private float explosionForce = 15f;
    [SerializeField] private LayerMask affectedLayers;
    
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    public void Damage(float damageAmount)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);
        foreach (Collider2D hitCollider in hitColliders)
        {
            IDamageable health = hitCollider.GetComponent<IDamageable>();
            if (health != null && hitCollider.gameObject != this.gameObject)
            {
                health.Damage(explosionDamage);
            }
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (hitCollider.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }
        
        Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    
}
