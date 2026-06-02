using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;

        base.Awake();
    }

    public override void Start()
    {
        loginPanel = GetComponent<LoginPanel>();

        base.Start();
    }

    public override void OnResponse(MainPack pack)
    {
        bool isSuccessful = false;
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeeded:
                isSuccessful = true;
                break;
            case ReturnCode.Failed:
                isSuccessful = false;
                break;
        }

        //切换到主线程
        mainContext.Post(_ => loginPanel.ShowAuthTooltip(isSuccessful), null);
    }

    public void SendRequest(string username, string password)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        AuthPack loginPack = new AuthPack();
        loginPack.Username = username;
        pack.AuthPack = loginPack;

        base.SendRequest(pack);
    }
}
