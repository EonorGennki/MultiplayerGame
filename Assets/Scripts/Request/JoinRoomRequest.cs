using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;

        base.Awake();
    }

    public override void Start()
    {
        roomListPanel = GetComponent<RoomListPanel>();

        base.Start();
    }

    public override void OnResponse(MainPack pack)
    {
        bool success;
        string str;
        List<PlayerInfo> playerList = new List<PlayerInfo>();
        RoomInfo roomInfo = new RoomInfo();
        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                success = true;
                str = "加入成功";
                UpdatePlayerList(playerList, pack);
                UpdateRoomInfo(roomInfo, pack);
                break;
            case ReturnCode.Failure:
                success = false;
                str = "加入失败";
                playerList = null;
                break;
            default:
                success = false;
                str = "请求异常";
                playerList = null;
                break;
        }

        //切换到主线程
        mainContext.Post(_ => roomListPanel.ShowRoomTooltip<JoinRoomRequest>(success, str, playerList, roomInfo), null);
    }

    public void SendRequest(string roomName)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack room = new RoomPack();
        room.RoomName = roomName;
        pack.RoomPack.Add(room);
        base.SendRequest(pack);
    }

    private void UpdatePlayerList(List<PlayerInfo> playerList, MainPack pack)
    {
        foreach (var player in pack.PlayerPack)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.playerName = player.PlayerName;
            playerList.Add(playerInfo);
        }
    }

    private void UpdateRoomInfo(RoomInfo roomInfo, MainPack pack)
    {
        roomInfo.roomName = pack.RoomPack[0].RoomName;
        roomInfo.currentNum = pack.RoomPack[0].CurrentNum;
        roomInfo.maxNum = pack.RoomPack[0].MaxNum;
        roomInfo.state = pack.RoomPack[0].StateCode.ToString();
    }
}
