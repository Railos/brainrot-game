using UnityEngine;

public class EnemyDoDamageTrigger : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    [SerializeField] private float damage;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == PlayerTarget)
        {
            PlayerTarget.GetComponent<IDamageable>().Damage(damage);
        }
    }
}
