using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float moveSpeed;

    private SpriteRenderer _spriteRenderer;
    private Transform _transform;
    private Weapon _weapon;

    private Vector2 _moveDirection = Vector2.zero;
    private Vector3 _aimPosition = Vector3.zero;

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _weapon = GetComponentInChildren<Weapon>();
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

    public void Fire() => _weapon.Fire();

    public void SetMoveDirection(Vector2 direction) => _moveDirection = direction;
    public void SetAimPosition(Vector3 direction)
    {
        _aimPosition = direction;
        _weapon.SetAimPosition(_aimPosition);
    }

    private void FlipSprite()
    {
        if (_aimPosition.x > _transform.position.x && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_aimPosition.x < _transform.position.x && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
    }

}
