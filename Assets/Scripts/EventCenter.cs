using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    public delegate void HealthChangedHandler(long playerId, int health);
    public event HealthChangedHandler OnHealthChanged;

    public void TriggerHealthChanged(long playerId, int health)
    {
        OnHealthChanged?.Invoke(playerId, health);
    }
}
