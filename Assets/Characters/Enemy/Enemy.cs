
using System;
using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField] private float damage;
    
    #region UnityEvents
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damage);
        }
    }

    #endregion
    
    // #region PublicMethods
    //
    //
    // #endregion
    
    #region ProtectedMethods
    
    protected override void Rotate()
    {
        SpriteRenderer.flipX = SpriteRenderer.flipX switch
        {
            true when Rigidbody2D.velocity.x < 0 => false,
            false when Rigidbody2D.velocity.x > 0 => true,
            _ => SpriteRenderer.flipX
        };
    }
    
    #endregion
}
