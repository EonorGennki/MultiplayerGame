 using SocketGameProtocal;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoSingleton<GameFacade>
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uiManager;
    private PlayerManeger playerManeger;
    private InputManager inputManager;

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
        inputManager = new InputManager();
    }

    void Start()
    {
        clientManager.OnInit();
        requestManager.OnInit();
        uiManager.OnInit();
        playerManeger.OnInit();
        inputManager.OnInit();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uiManager.OnDestroy();
        playerManeger.OnDestroy();
        inputManager.OnDestroy();
    }

    public void Send(MainPack pack) => clientManager.Send(pack);

    public void HandleResponse(MainPack pack) => requestManager.HandleResponse(pack);

    public void AddRequest(BaseRequest request) => requestManager.AddRequest(request);

    public void RemoveRequest(ActionCode action) => requestManager.RemoveRequest(action);

    public void AddPlayer(List<PlayerInfo> playerList) => playerManeger.AddPlayer(playerList);

    public void RemovePlayer(long playerId) => playerManeger.RemovePlayer(playerId);

    public void ShowLeaveGamePanel() => uiManager.ShowLeaveGamePanel();

    /// <summary>
    /// 自动离开游戏
    /// </summary>
    public void AutoLeaveGame()
    {
        playerManeger.Clear();
        uiManager.PopPanel();
        uiManager.PopPanel();
    }
}
