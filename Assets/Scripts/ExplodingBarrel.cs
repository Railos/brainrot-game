using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 30;
    [SerializeField] private float explosionForce = 15f;
    [SerializeField] private LayerMask affectedLayers;

    private void Start()
    {
        GetComponent<Health>().actionAfterDeath += Explode;
    }
    
    public void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);
        foreach (Collider2D hitCollider in hitColliders)
        {
            Health health = hitCollider.GetComponent<Health>();
            if (health != null && health != GetComponent<Health>())
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
        
        Destroy(this.gameObject);
    }
}
