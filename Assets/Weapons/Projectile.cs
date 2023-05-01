using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;

    protected virtual void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0) Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Enemy")) Destroy(gameObject);
    }
}
