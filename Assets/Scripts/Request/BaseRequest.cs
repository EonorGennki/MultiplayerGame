using SocketGameProtocal;
using System.Threading;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    protected GameFacade facade;
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
        facade = GameFacade.Instance;
        facade.AddRequest(this);
    }

    public virtual void OnDestroy()
    {
        facade.RemoveRequest(actionCode);
    }

    public virtual void OnResponse(MainPack pack)
    {

    }

    public virtual void SendRequest(MainPack pack)
    {
        facade.Send(pack);
    }
}
