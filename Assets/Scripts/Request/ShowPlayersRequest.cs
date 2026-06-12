using SocketGameProtocal;
using UnityEngine;

public class ShowPlayersRequest : BaseRequest
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
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.playerName = player.PlayerName;
            roomPanel.playerList.Add(playerInfo);
            UpdateRoomInfo(pack, roomInfo);
        }

        mainContext.Post(_ =>
        {
            roomPanel.ShowPlayerList();
            roomPanel.UpdateRoomInfo(roomInfo);
        }, null);
    }

    private void UpdateRoomInfo(MainPack pack, RoomInfo roomInfo)
    {
        roomInfo.roomName = pack.RoomPack[0].RoomName;
        roomInfo.currentNum = pack.RoomPack[0].CurrentNum;
        roomInfo.maxNum = pack.RoomPack[0].MaxNum;
    }
}
