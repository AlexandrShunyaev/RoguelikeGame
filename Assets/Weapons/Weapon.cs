using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float fireForce;
    [SerializeField] protected Transform firePoint;
    
    protected Vector3 AimPosition;

    public void SetAimPosition(Vector3 direction) => AimPosition = direction;
    
    public void Fire()
    {
        var position = firePoint.position;
        var projectile = Instantiate(projectilePrefab, position, firePoint.rotation);
        var rb = projectile.GetComponent<Rigidbody2D>();
        var direction = (AimPosition - position);
        rb.AddForce(new Vector2(direction.x, direction.y).normalized * fireForce, ForceMode2D.Impulse);
    }
}
