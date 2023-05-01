using UnityEngine;

public class DefaultProjectile : Projectile
{


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
