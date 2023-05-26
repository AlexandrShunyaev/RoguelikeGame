using UnityEngine;

public sealed class CommonEnemyAgent : EnemyAgent
{
    private int countOfWallTouch = 3;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<CompositeCollider2D>(out var wall))
        {
            UpdateReward(-5*EpisodeTimer);
            EndCurrentEpisode();
        }
        if (other.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            UpdateReward(-3f);
        }
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            UpdateReward(5*EpisodeTimer);
            EndCurrentEpisode();
        }
    }
}
