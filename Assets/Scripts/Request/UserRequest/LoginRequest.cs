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
        bool success;
        string str;

        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                success = true;
                str = "登陆成功";
                OnLoginSuccess(pack);
                break;
            case ReturnCode.Failure:
                success = false;
                str = " 登陆失败";
                break;
            default:
                success = false;
                str = "请求异常";
                break;
        }

        //切换到主线程
        mainContext.Post(_ => loginPanel.ShowAuthTooltip(success, str), null);
    }

    public void SendRequest(string username, string password)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        AuthPack loginPack = new AuthPack();
        loginPack.Username = username;
        loginPack.Password = password;
        pack.AuthPack = loginPack;

        base.SendRequest(pack);
    }

    private void OnLoginSuccess(MainPack pack)
    {
        
    }
}
