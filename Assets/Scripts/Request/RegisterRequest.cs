using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    private RegisterPanel registerPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;

        base.Awake();
    }

    public override void Start()
    {
        registerPanel = GetComponent<RegisterPanel>();
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
                str = "注册成功";
                break;
            case ReturnCode.Failure:
                success = false;
                str = " 注册失败";
                break;
            default:
                success = false;
                str = "请求异常误";
                break;
        }

        //切换到主线程
        mainContext.Post(_ => registerPanel.ShowAuthTooltip(success, str), null);
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
