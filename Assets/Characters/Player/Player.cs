using UnityEngine;

public class Player : Character
{
    private Weapon _weapon;

    #region UnityEvents
    
    protected override void Awake()
    {
        base.Awake();
        
        _weapon = GetComponentInChildren<Weapon>();
    }

    protected override void Update()
    {
        base.Update();
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    #endregion

    #region PublicMethods

    public override void Attack() => _weapon.Fire(AimDirection);
    
    public override void SetAimDirection(Vector2 direction)
    {
        base.SetAimDirection(direction);
        _weapon.Rotate(AimDirection);
    }
    
    #endregion
    
    #region ProtectedMethods
    
    protected override void Rotate()
    {
        if (AimDirection.normalized.x > 0 && SpriteRenderer.flipX) SpriteRenderer.flipX = false;
        else if (AimDirection.normalized.x < 0 && !SpriteRenderer.flipX) SpriteRenderer.flipX = true;
    }
    
    #endregion


}
