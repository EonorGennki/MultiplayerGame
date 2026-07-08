using Google.Protobuf;
using SocketGameProtocal;
using System;
using System.Linq;
using UnityEngine;

public class Message
{
    private byte[] buffer = new byte[1024];
    private int startIndex;

    public byte[] Buffer
    {
        get { return buffer; }
    }

    public int StartIndex
    {
        get { return startIndex; }
    }

    public int RemSize
    {
        get { return buffer.Length - startIndex; }
    }

    /// <summary>
    /// 解析消息
    /// </summary>
    /// <param name="len"></param>
    /// <param name="HandleResponse"></param>
    public void ReadBuffer(int len, Action<MainPack> HandleResponse)
    {
        startIndex += len;

        //数据不完整，等待更多数据
        if (startIndex <= 4)
        {
            return;
        }

        int count = BitConverter.ToInt32(buffer, 0);

        while (startIndex >= 4)
        {
            //解析数据
            MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);
            HandleResponse(pack);

            //移除已处理数据
            Array.Copy(buffer, count + 4, buffer, 0, startIndex - count - 4);
            startIndex -= count + 4;
        }
    }

    /// <summary>
    /// 包装数据
    /// </summary>
    /// <param name="pack"></param>
    /// <returns></returns>
    public static byte[] PackData(MainPack pack)
    {
        byte[] data = pack.ToByteArray(); //包体
        byte[] head = BitConverter.GetBytes(data.Length); //包头
        return head.Concat(data).ToArray();
    }
}
