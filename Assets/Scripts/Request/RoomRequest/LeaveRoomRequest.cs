using SocketGameProtocal;
using UnityEngine;

public class LeaveRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.LeaveRoom;

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
        mainContext.Post(_ => roomPanel.AutoLeaveRoom(), null);
    }
}
