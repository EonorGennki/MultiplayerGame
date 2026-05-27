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

    public void AddRequest(BaseRequest _request)
    {
        requestDic.Add(_request.GetActionCode, _request);
    }

    public void RemoveRequest(ActionCode _actionCode)
    {
        requestDic.Remove(_actionCode);
    }

    public void HandleResponse(MainPack _pack)
    {
        if (requestDic.TryGetValue(_pack.ActionCode, out BaseRequest request))
        {
            request.OnResponse(_pack);
        }
        else
        {
            Debug.LogWarning("不能找到对应的处理");
        }
    }
}
