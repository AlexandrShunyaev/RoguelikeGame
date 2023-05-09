using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public enum AgentMode
{
    Training,
    Play
}

public class CommonEnemyTeacher : Agent
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private AgentMode mode = AgentMode.Training;
    [SerializeField] private Transform target;
    [SerializeField] private List<Transform> sensors;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;
    private Vector2 _moveDirection;
    private Vector3 _defaultPosition;
    private float _timer = 10.0f;
    

    private static float[] _directionAxis = 
        { -1.0f, -0.8f, -0.6f, -0.4f, -0.2f, 0, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
    
    protected override void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _transform = transform;
        _defaultPosition = transform.position;
    }

    private void Update()
    {
        Flip();
    }
    
    private void FixedUpdate()
    {
        UpdateReward(0.001f);
        _rigidbody2D.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        _moveDirection = Vector2.zero;
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _timer = 10.0f;
            UpdateReward(-0.009f);
        }
    }

    public override void OnEpisodeBegin()
    {
        switch (mode)
        {
            case AgentMode.Training:
                transform.position = _defaultPosition;
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_rigidbody2D.velocity);
        sensor.AddObservation(_transform.position);
        sensor.AddObservation(target.position);

        foreach (var s in sensors)
        {
            var hit = Physics2D.Raycast(s.position, s.up);
            Debug.DrawRay(s.position, s.up, Color.red);
            sensor.AddObservation(hit.distance);
            sensor.AddObservation(hit.collider.gameObject.CompareTag("Environment"));
            sensor.AddObservation(hit.collider.gameObject.CompareTag("Player"));
            sensor.AddObservation(hit.collider.gameObject.CompareTag("Enemy"));
            sensor.AddObservation(hit.collider.gameObject.CompareTag("Projectile"));
            sensor.AddObservation(hit.collider.gameObject.transform.position);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        SetMoveDirection(
            new Vector2(
                _directionAxis[actions.DiscreteActions[0]],
                _directionAxis[actions.DiscreteActions[1]]
            )
        );
    }

    public void SetMoveDirection(Vector2 direction) => _moveDirection = direction;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var collisionTag = other.gameObject.tag;
        if (collisionTag == "Player")
        {
            UpdateReward(1.0f);
            EndEpisode();
        }
        else if(collisionTag is "Environment")
        {
            UpdateReward(-0.6f);
            EndEpisode();
        }
        else if (collisionTag is "Enemy")
        {
            UpdateReward(-0.06f);
        }
        else if (collisionTag is "Projectile")
        {
            UpdateReward(-0.05f);
        }
    }

    private void UpdateReward(float value)
    {
        AddReward(value);
        Debug.Log(value);
    }

    private void Flip()
    {
        if (_spriteRenderer.flipX && _rigidbody2D.velocity.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (!_spriteRenderer.flipX && _rigidbody2D.velocity.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
