using SocketGameProtocal;
using System;
using System.Net.Sockets;
using UnityEngine;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;

    public ClientManager(GameFace _face) : base(_face)
    {
    }

    public override void OnInit()
    {
        base.OnInit();

        message = new Message();
        InitSocket();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        message = null;
        CloseSocket();
    }

    /// <summary>
    /// ≥ı ºªØsocket
    /// </summary>
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("127.0.0.1", 6666);
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    /// <summary>
    /// πÿ±’socket
    /// </summary>
    private void CloseSocket()
    {
        if (socket.Connected && socket is not null)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.RemSize, SocketFlags.None, ReceiveCallBack, null);
    }

    private void ReceiveCallBack(IAsyncResult _result)
    {
        try
        {
            int _len = socket.EndReceive(_result);
            if (_len == 0 && socket.Connected == false)
            {
                CloseSocket();
                return;
            }

            message.ReadBuffer(_len, HandleResponse);
        }
        catch
        {

        }
    }

    private void HandleResponse(MainPack _pack)
    {
        face.HandleResponse(_pack);
    }

    public void Send(MainPack _pack)
    {
        socket.Send(Message.PackData(_pack));
    }
}
