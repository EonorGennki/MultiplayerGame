using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI readyText;
    private bool isReady;
    private int userId;

    public void SetPlayerInfo(PlayerInfo player)
    {
        playerName.text = player.PlayerName;
        isReady = player.IsReady;
        userId = player.UserId;
        if (isReady)
        {
            readyText.text = "已准备";
            readyText.color = Color.green;
        }
        else
        {
            readyText.text = "未准备";
            readyText.color = Color.red;
        }
    }

    public PlayerInfo GetPlayerInfo() => new PlayerInfo(userId, playerName.text, isReady);
}
