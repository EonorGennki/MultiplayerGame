using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealthRequest : BaseRequest
{
    private PlayerHealth playerHealth;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateHealth;

        base.Awake();
    }

    public override void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        base.Start();
    }

    public void SendRequest(long playerId, int deltaHealth, long attackPlayerId = -1)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        PlayerPack playerPack = new PlayerPack();
        playerPack.PlayerId = playerId;
        playerPack.DeltaHealth = deltaHealth;
        playerPack.AttackPlayerId = attackPlayerId;

        pack.PlayerPack.Add(playerPack);

        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        PlayerPack playerPack = pack.PlayerPack[0];
        long playerId = playerPack.PlayerId;
        int health = playerPack.Health;
        bool isDead = playerPack.IsDead;
        mainContext.Post(_ => playerHealth.UpdateHealth(playerId, health, isDead), null);
    }
}
