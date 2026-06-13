using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocal;

public class ChatRequest : BaseRequest
{
    private RoomPanel roomPanel;
    private string text = string.Empty;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Chat;

        base.Awake();
    }

    public override void Start()
    {
        roomPanel = GetComponent<RoomPanel>();

        base.Start();
    }

    public void SendRequest(string text)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Text = text;
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        text = pack.Text;

        mainContext.Post(_ => roomPanel.UpdateChatList(text), null);
    }
}
