using SocketGameProtocal;
using System.Threading;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    protected GameFace face;
    protected SynchronizationContext mainContext;

    public ActionCode ActionCode
    {
        get
        {
            return actionCode;
        }
    }

    public virtual void Awake()
    {
        mainContext = SynchronizationContext.Current;
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

    public virtual void OnResponse(MainPack pack)
    {

    }

    public virtual void SendRequest(MainPack pack)
    {
        face.Send(pack);
    }
}
