using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float lifeTime;

    protected void Update()
    {
        DestroyByTime();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(damage); 
            Destroy(gameObject);
        }
        else if (other.gameObject.TryGetComponent<CompositeCollider2D>(out var wall))
        {
            Destroy(gameObject);
        }
    }

    private void DestroyByTime()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0) Destroy(gameObject);
    }
}
