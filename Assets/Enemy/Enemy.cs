
using Unity.MLAgents;
using UnityEngine;

public class Enemy : Agent
{
   [SerializeField] protected int health = 100;
   [SerializeField] protected float moveSpeed = 4f;
}
