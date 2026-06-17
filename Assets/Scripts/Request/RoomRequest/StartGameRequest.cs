using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class StartGameRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.StartGame;

        base.Awake();
    }

    public override void Start()
    {
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
        
    }
}
