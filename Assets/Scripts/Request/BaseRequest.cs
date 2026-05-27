using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    protected GameFace face;

    public ActionCode GetActionCode
    {
        get
        {
            return actionCode;
        }
    }

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {
        face = GameFace.Instance;
        face.AddRequest(this);
    }

    public virtual void OnDestroy()
    {
        face.RemoveRequest(actionCode);
    }

    public virtual void OnResponse(MainPack _pack)
    {

    }

    public virtual void SendRequest(MainPack _pack)
    {
        face.Send(_pack);
    }
}
