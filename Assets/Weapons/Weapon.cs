using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float fireForce;
    [SerializeField] protected Transform firePoint;
    protected SpriteRenderer SpriteRenderer;

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Rotate(Vector2 aimDirection)
    {
        var rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (aimDirection.normalized.x < 0 && !SpriteRenderer.flipY) SpriteRenderer.flipY = true;
        else if (aimDirection.normalized.x > 0 && SpriteRenderer.flipY) SpriteRenderer.flipY = false;

    }

    public abstract void Fire(Vector2 aimDirection);
}
