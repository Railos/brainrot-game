using System;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;
    private void Awake()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
