using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float moveSpeed;

    private SpriteRenderer _spriteRenderer;
    private Transform _transform;

    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _lookDirection = Vector2.zero;

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        FlipSprite();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        _moveDirection = Vector2.zero;
    }

    public void SetMoveDirection(Vector2 direction) => _moveDirection = direction;
    public void SetLookDirection(Vector2 direction) => _lookDirection = direction;

    private void FlipSprite()
    {
        if (_lookDirection.x > _transform.position.x && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_lookDirection.x < _transform.position.x && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
    }

}
