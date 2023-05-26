using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private static readonly float[] AxisValues = { -1.0f, -0.8f, -0.6f, -0.4f, -0.2f, 0, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
    
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void SetMoveDirection(int x, int y) => _enemy.SetMoveDirection(new Vector2(AxisValues[x], AxisValues[y]));

    public void SetAimDirection(int x, int y) => _enemy.SetMoveDirection(new Vector2(AxisValues[x], AxisValues[y]));

    public void Attack() => _enemy.Attack();
}
