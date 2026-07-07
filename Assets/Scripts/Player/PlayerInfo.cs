using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public long PlayerId { get; }
    public string PlayerName { get; }
    public bool IsReady {  get; set; }
    public int Health { get; set; }
    public Vector2 Position { get; set; }

    public PlayerInfo(long playerId, string playerName, bool isReady, int health = 0)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        IsReady = isReady;
        Health = health;
    }
}
