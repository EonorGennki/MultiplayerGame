using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGameRequest : BaseRequest
{
    private InGamePanel inGamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.LeaveGame;

        base.Awake();
    }

    public override void Start()
    {
        inGamePanel = GetComponent<InGamePanel>();

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
        mainContext.Post(_ => facade.AutoLeaveGame(), null);
    }
}
