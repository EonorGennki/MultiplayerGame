using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class StartGameRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.StartGame;

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
        bool success;
        string str = "";

        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                success = true;
                break;
            case ReturnCode.Failure:
                success = false;
                str = WhyFalse(pack.ErrorCode);
                break;
            default:
                success = false;
                str = "返回码异常";
                break;
        }

        mainContext.Post(_ => roomPanel.ShowRoomTooltip(success, str), null);
    }

    private string WhyFalse(ErrorCode errorCode)
    {
        switch (errorCode)
        {
            case ErrorCode.PlayerNotReady:
                return "有玩家未准备";
            default:
                return "未知错误";
        }
    }
}
