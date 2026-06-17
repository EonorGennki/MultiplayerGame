using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public int UserId { get; }
    public string PlayerName { get; }
    public bool IsReady {  get; set; }

    public PlayerInfo(int userId, string playerName, bool isReady)
    {
        this.UserId = userId;
        this.PlayerName = playerName;
        this.IsReady = isReady;
    }
}
