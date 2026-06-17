using SocketGameProtocal;
using UnityEngine;

public class UpdatePlayerListRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        actionCode = ActionCode.ShowPlayers;

        base.Awake();
    }

    public override void Start()
    {
        roomPanel = GetComponent<RoomPanel>();

        base.Start();
    }

    public override void OnResponse(MainPack pack)
    {
        RoomInfo roomInfo = new RoomInfo();
        roomPanel.playerList.Clear();
        foreach (var player in pack.PlayerPack)
        {
            PlayerInfo playerInfo = ToPlayerInfo(player);
            roomPanel.playerList.Add(playerInfo);
            UpdateRoomInfo(pack, roomInfo);
        }

        mainContext.Post(_ =>
        {
            roomPanel.UpdatePlayerList();
            roomPanel.UpdateRoomInfo(roomInfo);
        }, null);
    }

    private PlayerInfo ToPlayerInfo(PlayerPack player)
    {
        int userId = player.UserId;
        string playerName = player.PlayerName;
        bool isReady = player.IsReady;
        PlayerInfo playerInfo = new PlayerInfo(userId, playerName, isReady);
        return playerInfo;
    }

    private void UpdateRoomInfo(MainPack pack, RoomInfo roomInfo)
    {
        roomInfo.roomName = pack.RoomPack[0].RoomName;
        roomInfo.currentNum = pack.RoomPack[0].CurrentNum;
        roomInfo.maxNum = pack.RoomPack[0].MaxNum;
    }
}
