using System;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public enum AgentMode
{
    Training,
    Playing
}

public abstract class EnemyAgent : Agent
{
    #region Fields
    [SerializeField]
    private AgentMode agentMode;

    private bool _isCloseToPlayer = false;
    private float _lastDistanceToPlayer = 10000f;
    protected float EpisodeTimer = 30f;
    
    private EnemyController _controller;
    private GameProcessor _gameProcessor;
    private GameObject _player;

    private readonly List<Vector2> _lookDirections = new List<Vector2>()
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
        new(-1, 1),
        new(-1, -1),
        new(1, 1),
        new(1, -1),
    };

    #endregion

    #region UnityEvents
    
    protected new virtual void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _gameProcessor = FindObjectOfType<GameProcessor>();
        _player = FindObjectOfType<Player>().gameObject;
    }

    private void FixedUpdate()
    {
        EpisodeTimer -= Time.deltaTime;
        
        if (EpisodeTimer <= 0)
        {
            EndCurrentEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        _lastDistanceToPlayer = 10000f;
        EpisodeTimer = 30f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // собираем данные
        var (playerPos, currentPos, distanceToPlayer, directionToPlayer, hitWallOnWayToPlayer) = GetState();
        _lastDistanceToPlayer = distanceToPlayer;
        // выставляем наблюдения
        sensor.AddObservation(distanceToPlayer);
        sensor.AddObservation(hitWallOnWayToPlayer.collider is not null ? directionToPlayer : Vector2.zero);
        sensor.AddObservation(hitWallOnWayToPlayer.collider is not null ? hitWallOnWayToPlayer.distance : -1);
        
        // собираем доп данные и наблюдения
        foreach (var lookDirection in _lookDirections)
        {
            var hitWall = Physics2D.Raycast(currentPos, lookDirection, 15, 0);
            var hitEnemy = Physics2D.Raycast(currentPos + (lookDirection*0.6f), lookDirection, 15, 4);

            sensor.AddObservation(hitWall.collider is not null ? hitWall.distance : -1);
            sensor.AddObservation(hitEnemy.collider is not null ? hitEnemy.distance : -1);
        }
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        _controller.SetMoveDirection(actions.DiscreteActions[0], actions.DiscreteActions[1]);
        
        var (playerPos, currentPos, distanceToPlayer, directionToPlayer, hitWallOnWayToPlayer) = GetState();

        if (distanceToPlayer < 10 && hitWallOnWayToPlayer.collider is null && !_isCloseToPlayer)
        {
            UpdateReward(3f);
            _isCloseToPlayer = true;
        }
        if (distanceToPlayer >= 10 && _isCloseToPlayer)
        {
            UpdateReward(-3.2f);
            _isCloseToPlayer = false;
        }

        if (_isCloseToPlayer && _lastDistanceToPlayer > distanceToPlayer)
        {
            UpdateReward(2f);
        }
        if (_isCloseToPlayer && _lastDistanceToPlayer < distanceToPlayer)
        {
            UpdateReward(-2.2f);
        }
    }

    #endregion

    #region ProtectedMethods

    protected void EndCurrentEpisode()
    {
        if(agentMode is AgentMode.Training) _gameProcessor.EpisodeEnd(gameObject);
        EndEpisode();
    }

    protected void UpdateReward(float value)
    {
        AddReward(value);
        //Debug.Log(value);
    }

    protected (Vector2,Vector2,float,Vector2,RaycastHit2D) GetState()
    {
        var playerPos = new Vector2(_player.transform.position.x, _player.transform.position.y);
        var currentPos = new Vector2(transform.position.x, transform.position.y);
        var distanceToPlayer = Vector2.Distance(currentPos, playerPos);
        var directionToPlayer = new Vector2(playerPos.x - currentPos.x, playerPos.y - currentPos.y).normalized;
        var hitWallOnWayToPlayer = Physics2D.Raycast(currentPos, directionToPlayer, distanceToPlayer, 0);

        return (playerPos, currentPos, distanceToPlayer, directionToPlayer, hitWallOnWayToPlayer);
    }

    #endregion
}
