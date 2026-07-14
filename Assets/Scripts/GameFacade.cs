 using SocketGameProtocal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameFacade : MonoSingleton<GameFacade>
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uiManager;
    private GameManeger gameManeger;
    private InputManager inputManager;

    public long LocalPlayerId
    {
        get => gameManeger.GetLocalPlayerId();
        set => gameManeger.SetLocalPlayerId(value);
    }

    protected override void Awake()
    {
        base.Awake();

        uiManager = new UIManager();
        clientManager = new ClientManager();
        requestManager = new RequestManager();
        gameManeger = new GameManeger();
        inputManager = new InputManager();
    }

    void Start()
    {
        clientManager.OnInit();
        requestManager.OnInit();
        uiManager.OnInit();
        gameManeger.OnInit();
        inputManager.OnInit();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uiManager.OnDestroy();
        gameManeger.OnDestroy();
        inputManager.OnDestroy();
    }

    public void Send(MainPack pack) => clientManager.Send(pack);

    public void HandleResponse(MainPack pack) => requestManager.HandleResponse(pack);

    public void AddRequest(BaseRequest request) => requestManager.AddRequest(request);

    public void RemoveRequest(ActionCode action) => requestManager.RemoveRequest(action);

    public void AddPlayer(List<PlayerInfo> playerList) => gameManeger.AddPlayer(playerList);

    public void RemovePlayer(long playerId) => gameManeger.RemovePlayer(playerId);

    public void ShowLeaveGamePanel() => uiManager.ShowLeaveGamePanel();

    /// <summary>
    /// 自动离开游戏
    /// </summary>
    public void AutoLeaveGame()
    {
        inputManager.SwitchMap("UI");
        gameManeger.Clear();
        uiManager.PopPanel();
        uiManager.PopPanel();
    }

    public void UpdateCharacterState(long playerId, StatePack statePack) => gameManeger.UpdateCharacterState(playerId, statePack);

    public void SwitchActionMap(string mapName) => inputManager.SwitchMap(mapName);

    public PlayerInputSet GetPlayerInput() => inputManager.PlayerInput;
}
