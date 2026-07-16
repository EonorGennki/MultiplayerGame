using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    public event Action<long, int> OnHealthChanged;
    public event Action<Transform> OnCameraFollow;
    public event Action<long, int> OnScoreChanged;

    public void TriggerHealthChanged(long playerId, int health)
    {
        OnHealthChanged?.Invoke(playerId, health);
    }

    public void TriggerOnCameraFollow(Transform player)
    {
        OnCameraFollow?.Invoke(player);
    }

    public void TriggerOnScoreChanged(long playerId, int score)
    {
        OnScoreChanged?.Invoke(playerId, score);
    }
}
