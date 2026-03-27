using SocketGameProtocal;
using UnityEngine;

public class GameFace : MonoBehaviour
{
    public ClientManager clientManager;

    void Start()
    {
        clientManager = new ClientManager(this);
        clientManager.OnInit();
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
    }

    public void Send(MainPack _pack)
    {
        clientManager.Send(_pack);
    }

    public void HandleResponse(MainPack _pack)
    {
        //¥¶¿Ì
    }
}
