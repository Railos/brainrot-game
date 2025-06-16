using System;
using UnityEngine;

public class EnemyAttackDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private Enemy _enemy;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == PlayerTarget)
        {
            _enemy.SetAttackStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == PlayerTarget)
        {
            _enemy.SetAttackStatus(false);
        }
    }
}
