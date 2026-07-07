using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanStartRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        actionCode = ActionCode.CanStart;

        base.Awake();
    }

    public override void Start()
    {
        roomPanel = GetComponent<RoomPanel>();  

        base.Start();
    }

    public override void OnResponse(MainPack pack)
    {
        List<PlayerInfo> playerList = ToPlayerList(pack);
        mainContext.Post(_ => {
            facade.AddPlayer(playerList);
            roomPanel.StartGame(playerList);
            }, null);
    }

    private List<PlayerInfo> ToPlayerList(MainPack pack)
    {
        List<PlayerInfo> playerList = new List<PlayerInfo>();
        foreach (var player in pack.PlayerPack)
        {
            PlayerInfo playerInfo = ToPlayerInfo(player);
            playerList.Add(playerInfo);
        }
        return playerList;
    }

    private PlayerInfo ToPlayerInfo(PlayerPack playerPack)
    {
        long playerId = playerPack.PlayerId;
        string playerName = playerPack.PlayerName;
        bool isReady = playerPack.IsReady;
        int health = playerPack.Health;
        PlayerInfo playerInfo = new PlayerInfo(playerId, playerName, isReady, health);
        return playerInfo;
    }
}
