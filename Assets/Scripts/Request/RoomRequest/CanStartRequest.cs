using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanStartRequest : BaseRequest
{
    public override void Awake()
    {
        actionCode = ActionCode.CanStart;

        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        mainContext.Post(_ => Debug.Log("start"), null);
    }
}
