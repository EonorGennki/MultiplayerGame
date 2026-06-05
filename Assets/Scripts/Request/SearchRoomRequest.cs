using SocketGameProtocal;

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
                str = "ËÑË÷³É¹¦";
                break;
            case ReturnCode.Failure:
                success = false;
                str = "ËÑË÷Ê§°Ü";
                break;
            default:
                success = false;
                str = "Î´Öª´íÎó";
                break;
        }

        foreach (RoomPack room in pack.RoomPack)
        {
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.roomName = room.RoomName;
            roomInfo.currentNum = room.CurrentNum;
            roomInfo.maxNum = room.MaxNum;
            roomInfo.state = room.StateCode.ToString();
            roomListPanel.roomInfoList.Add(roomInfo);
        }

        //ÇÐ»»µ½Ö÷Ïß³Ì
        mainContext.Post(_ => roomListPanel.ShowRoomTooltip<SearchRoomRequest>(success, str), null);
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        base.SendRequest(pack);
    }
}
