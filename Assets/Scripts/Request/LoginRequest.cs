using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
    }

    public void SendRequest(string username, string password)
    {

    }
}
