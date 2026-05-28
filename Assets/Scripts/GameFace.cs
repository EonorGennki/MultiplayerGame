using SocketGameProtocal;
using UnityEngine;

public class GameFace : MonoSingleton<GameFace>
{
    private ClientManager clientManager;
    private RequestManager requestManager;

    protected override void Awake()
    {
        base.Awake();

        clientManager = new ClientManager();
        requestManager = new RequestManager();
    }

    void Start()
    {
        clientManager.OnInit();
        requestManager.OnInit();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        clientManager.OnDestroy();
        requestManager.OnDestroy();
    }

    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void HandleResponse(MainPack pack)
    {
        requestManager.HandleResponse(pack);
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }
}
