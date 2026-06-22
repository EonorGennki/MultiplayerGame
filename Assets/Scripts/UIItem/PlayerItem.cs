using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI readyText;
    private bool isReady;
    private long playerId;

    public void SetPlayerInfo(PlayerInfo player)
    {
        playerName.text = player.PlayerName;
        isReady = player.IsReady;
        playerId = player.PlayerId;
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

    public PlayerInfo GetPlayerInfo() => new PlayerInfo(playerId, playerName.text, isReady);
}
