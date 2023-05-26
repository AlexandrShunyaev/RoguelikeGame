using UnityEngine;

public class DefaultWeapon : Weapon
{
    public override void Fire(Vector2 aimDirection)
    {
        var position = firePoint.position;
        var projectile = Instantiate(projectilePrefab, position, firePoint.rotation);
        var rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(aimDirection.x, aimDirection.y).normalized * fireForce, ForceMode2D.Impulse);
    }
}
