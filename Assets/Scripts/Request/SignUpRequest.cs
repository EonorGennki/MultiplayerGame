using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUpRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.SignUp;

        base.Awake();
    }

    public override void OnResponse(MainPack _pack)
    {
        switch(_pack.ReturnCode)
        {
            case ReturnCode.Succeeded:
                Debug.Log("×¢²á³É¹¦");
                break;
            case ReturnCode.Failed:
                Debug.Log("×¢²áÊ§°Ü");
                break;

        }
    }

    public void SendRequest(string _username, string _password)
    {
        LoginPack loginPack = new LoginPack();
        loginPack.Username = _username;
        loginPack.Password = _password;
        MainPack _pack = new MainPack();
        _pack.RequestCode = requestCode;
        _pack.ActionCode = actionCode;
        _pack.LoginPack = loginPack;

        base.SendRequest(_pack);
    }
}
