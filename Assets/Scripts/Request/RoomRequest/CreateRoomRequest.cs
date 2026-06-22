using SocketGameProtocal;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;

        base.Awake();
    }

    public override void Start()
    {
        roomListPanel = GetComponent<RoomListPanel>();

        base.Start();
    }

    public void SendRequest(string roomName, int maxNum)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack roomPack = ToRoomPack(roomName, maxNum);
        pack.RoomPack.Add(roomPack);

        base.SendRequest(pack);
    }

    private RoomPack ToRoomPack(string roomName, int maxNum)
    {
        RoomPack roomPack = new RoomPack();
        roomPack.RoomName = roomName;
        roomPack.MaxNum = maxNum;
        roomPack.StateCode = StateCode.Waiting;
        return roomPack;
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
                str = "创建成功";
                UpdatePlayerList(playerList, pack);
                UpdateRoomInfo(roomInfo, pack);
                break;
            case ReturnCode.Failure:
                success = false;
                str = "创建失败";
                playerList = null;
                break;
            default:
                success = false;
                str = "请求异常";
                playerList = null;
                break;
        }

        //切换到主线程
        mainContext.Post(_ => roomListPanel.ShowRoomTooltip<CreateRoomRequest>(success, str, playerList, roomInfo), null);
    }

    private void UpdatePlayerList(List<PlayerInfo> playerList, MainPack pack)
    {
        foreach (var player in pack.PlayerPack)
        {
            PlayerInfo playerInfo = ToPlayerInfo(player);
            playerList.Add(playerInfo);
        }
    }

    private PlayerInfo ToPlayerInfo(PlayerPack player)
    {
        long userId = player.PlayerId;
        string playerName = player.PlayerName;
        bool isReady = player.IsReady;
        PlayerInfo playerInfo = new PlayerInfo(userId, playerName, isReady);
        return playerInfo;
    }

    private void UpdateRoomInfo(RoomInfo roomInfo, MainPack pack)
    {
        roomInfo.roomName = pack.RoomPack[0].RoomName;
        roomInfo.currentNum = pack.RoomPack[0].CurrentNum;
        roomInfo.maxNum = pack.RoomPack[0].MaxNum;
        roomInfo.state = pack.RoomPack[0].StateCode.ToString();
    }
}
