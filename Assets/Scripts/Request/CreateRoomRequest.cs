using SocketGameProtocal;
using System.Collections;
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

    public override void OnResponse(MainPack pack)
    {

        bool success;
        string str;
        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                success = true;
                str = "创建成功";
                break;
            case ReturnCode.Failure:
                success = false;
                str = "创建失败";
                break;
            default:
                success = false;
                str = "未知错误";
                break;
        }

        //切换到主线程
        mainContext.Post(_=> roomListPanel.ShowRoomTooltip<CreateRoomRequest>(success, str), null);
    }

    public void SendRequest(string roomName, int maxNum)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.RoomName = roomName;
        roomPack.MaxNum = maxNum;
        pack.RoomPack.Add(roomPack);

        base.SendRequest(pack);
    }
}
