using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    private RegisterPanel RegisterPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;

        base.Awake();
    }

    public override void Start()
    {
        RegisterPanel = GetComponent<RegisterPanel>();
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
        mainContext.Post(_ => RegisterPanel.ShowAuthTooltip(isSuccessful), null);
    }

    public void SendRequest(string username, string password)
    {
        AuthPack loginPack = new AuthPack();
        loginPack.Username = username;
        loginPack.Password = password;
        MainPack _pack = new MainPack();
        _pack.RequestCode = requestCode;
        _pack.ActionCode = actionCode;
        _pack.AuthPack = loginPack;

        base.SendRequest(_pack);
    }
}
