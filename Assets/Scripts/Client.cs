using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

public class Client : MonoBehaviour
{
    private Socket socket;
    private byte[] buffer = new byte[1024];

    // Start is called before the first frame update
    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect("127.0.0.1", 6666); //连接服务端
        StartReceive(); //开始接受服务器消息
        Send(); //发送一条消息给服务器
    }

    void StartReceive()
    {
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    void ReceiveCallback(IAsyncResult result)
    {
        int len = socket.EndReceive(result);

        if (len == 0)
        {
            return;
        }

        string str = Encoding.UTF8.GetString(buffer, 0, len);
        Debug.Log(str);

        StartReceive();
    }

    void Send()
    {
        socket.Send(Encoding.UTF8.GetBytes("你好！"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
