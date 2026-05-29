using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : BaseManager
{
    private Dictionary<ActionCode, BaseRequest> requestDic = new Dictionary<ActionCode, BaseRequest>();

    public RequestManager() : base()
    {
    }

    public void AddRequest(BaseRequest request)
    {
        requestDic.Add(request.ActionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestDic.Remove(actionCode);
    }

    public void HandleResponse(MainPack pack)
    {
        if (requestDic.TryGetValue(pack.ActionCode, out BaseRequest request))
        {
            request.OnResponse(pack);
        }
        else
        {
            Debug.LogWarning("不能找到对应的处理");
        }
    }
}
