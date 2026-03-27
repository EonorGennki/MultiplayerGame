using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketGameProtocal;
using Google.Protobuf;

public class Message : MonoBehaviour
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
    /// <param name="_len"></param>
    /// <param name="HandleResponse"></param>
    public void ReadBuffer(int _len, Action<MainPack> HandleResponse)
    {
        startIndex += _len;

        if (startIndex <= 4)
        {
            return;
        }

        int _count = BitConverter.ToInt32(buffer, 0);

        while (startIndex >= _count + 4)
        {
            MainPack _pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, _count);
            HandleResponse(_pack);
            Array.Copy(buffer, _count + 4, buffer, 0, startIndex - _count - 4);
            startIndex -= _count + 4;
        }
    }

    /// <summary>
    /// 包装数据
    /// </summary>
    /// <param name="pack"></param>
    /// <returns></returns>
    public static byte[] PackData(MainPack pack)
    {
        byte[] _data = pack.ToByteArray(); //包体
        byte[] _head = BitConverter.GetBytes(_data.Length); //包头
        return _head.Concat(_data).ToArray();
    }
}
