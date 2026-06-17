using SocketGameProtocal;
using UnityEngine;

public class SearchRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.SearchRoom;

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

        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                success = true;
                str = "成功搜索到" + pack.RoomPack.Count + "房间";
                break;
            case ReturnCode.Failure:
                success = false;
                str = WhyFalse(pack.ErrorCode);
                break;
            default:
                success = false;
                str = "请求异常";
                break;
        }

        UpdateRoomList(pack);

        //切换到主线程
        mainContext.Post(_ => roomListPanel.ShowRoomTooltip<SearchRoomRequest>(success, str), null);
    }

    private void UpdateRoomList(MainPack pack)
    {
        roomListPanel.roomList.Clear();
        foreach (RoomPack room in pack.RoomPack)
        {
            RoomInfo roomInfo = ToRoomPack(room);
            roomListPanel.roomList.Add(roomInfo);
        }
    }

    private RoomInfo ToRoomPack(RoomPack room)
    {
        RoomInfo roomInfo = new RoomInfo();
        roomInfo.roomName = room.RoomName;
        roomInfo.currentNum = room.CurrentNum;
        roomInfo.maxNum = room.MaxNum;
        roomInfo.state = room.StateCode.ToString();
        return roomInfo;
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        base.SendRequest(pack);
    }

    private string WhyFalse(ErrorCode errorCode)
    {
        switch (errorCode)
        {
            case ErrorCode.NotFound:
                return "未找到房间";
            default:
                return "未知错误";
        }
    }
}
