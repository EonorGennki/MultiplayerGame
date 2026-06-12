using SocketGameProtocal;
using System.Collections.Generic;

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

    public void SendRequest(string roomName, int maxNum)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.RoomName = roomName;
        roomPack.MaxNum = maxNum;
        roomPack.StateCode = StateCode.Waiting;
        pack.RoomPack.Add(roomPack);

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
