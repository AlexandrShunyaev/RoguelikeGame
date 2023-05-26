using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("State")]
    [SerializeField] protected float health;
    [SerializeField] protected float moveSpeed;
    
    protected Rigidbody2D Rigidbody2D;
    protected SpriteRenderer SpriteRenderer;
    
    protected Vector2 MoveDirection = Vector2.zero;
    protected Vector2 AimDirection = Vector2.zero;

    #region UnityEvents

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Rotate();

        if (health <= 0) Die();
    }
    
    protected virtual void FixedUpdate()
    {
        Move();
    }

    #endregion

    #region PublicMethods

    public void TakeDamage(float value)
    {
        health -= value;
    }
    
    public abstract void Attack();
    
    public virtual void SetMoveDirection(Vector2 direction) => MoveDirection = direction;
    
    public virtual void SetAimDirection(Vector2 direction) => AimDirection = direction;
    
    
    #endregion

    #region ProtectedMethods

    protected virtual void Move()
    {
        Rigidbody2D.velocity = new Vector2(MoveDirection.x * moveSpeed, MoveDirection.y * moveSpeed);
        MoveDirection = Vector2.zero;
    }

    protected abstract void Rotate();
    
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    #endregion

}
