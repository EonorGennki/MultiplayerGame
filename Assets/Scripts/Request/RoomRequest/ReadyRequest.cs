using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class ReadyRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Ready;

        base.Awake();
    }

    public override void Start()
    {
        roomPanel = GetComponent<RoomPanel>();

        base.Start();
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        PlayerPack playerPack = pack.PlayerPack[0];
        PlayerInfo player = ToPlayerInfo(playerPack);

        mainContext.Post(_ => roomPanel.UpdatePlayersState(player), null);
    }

    private PlayerInfo ToPlayerInfo(PlayerPack player)
    {
        long playerId = player.PlayerId;
        string playerName = player.PlayerName;
        bool isReady = player.IsReady;
        return new PlayerInfo(playerId, playerName, isReady);
    }
}
