 using SocketGameProtocal;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoSingleton<GameFacade>
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uiManager;
    private PlayerManeger playerManeger;

    public long LocalPlayerId
    {
        get => playerManeger.GetLocalPlayerId();
        set => playerManeger.SetLocalPlayerId(value);
    }

    protected override void Awake()
    {
        base.Awake();

        uiManager = new UIManager();
        clientManager = new ClientManager();
        requestManager = new RequestManager();
        playerManeger = new PlayerManeger();
    }

    void Start()
    {
        uiManager.OnInit();
        clientManager.OnInit();
        requestManager.OnInit();
        playerManeger.OnInit();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uiManager.OnDestroy();
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

    public void AddPlayer(List<PlayerInfo> playerList)
    {
        playerManeger.AddPlayer(playerList);
    }

    public void RemovePlayer(int playerId)
    {
        playerManeger.RemovePlayer(playerId);
    }
}
