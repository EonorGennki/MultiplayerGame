using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainScoreRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GainScore;

        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        PlayerPack playerPack = pack.PlayerPack[0];
        long attackPlayerId = playerPack.AttackPlayerId;
        int score = playerPack.Score;

        mainContext.Post(_ => facade.EventCenter.TriggerOnScoreChanged(attackPlayerId, score), null);
    }
}
