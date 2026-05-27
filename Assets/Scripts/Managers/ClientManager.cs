using SocketGameProtocal;
using System;
using System.Net.Sockets;
using UnityEngine;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;

    public ClientManager() : base()
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
        Close();
    }

    /// <summary>
    /// 初始化socket
    /// </summary>
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("127.0.0.1", 6666);
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    private void Close()
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
            if (socket == null || !socket.Connected)
            {
                return;
            }

            int _len = socket.EndReceive(_result);
            if (_len == 0)
            {
                Close();
                return;
            }

            message.ReadBuffer(_len, HandleResponse);
            StartReceive();
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    /// <summary>
    /// 处理响应
    /// </summary>
    /// <param name="_pack"></param>
    private void HandleResponse(MainPack _pack)
    {
        face.HandleResponse(_pack);
    }

    public void Send(MainPack _pack)
    {
        socket.Send(Message.PackData(_pack));
    }
}
