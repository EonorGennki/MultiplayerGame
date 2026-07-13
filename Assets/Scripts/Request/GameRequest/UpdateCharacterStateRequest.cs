using SocketGameProtocal;
using UnityEngine;

public class UpdateCharacterStateRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateCharacterState;
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public void SendRequest(StatePack statePack)
    {
        PlayerStatePack playerStatePack = new PlayerStatePack();
        playerStatePack.PosX = statePack.playerPos.x;
        playerStatePack.PosY = statePack.playerPos.y;
        playerStatePack.XVelocity = statePack.velocity.x;
        playerStatePack.YVelocity = statePack.velocity.y;
        playerStatePack.AnimeName = statePack.animeName;
        playerStatePack.IsFlip = statePack.isFlip;

        GunRotPack gunRotPack = new GunRotPack();
        gunRotPack.PosX = statePack.aimTargetPos.x;
        gunRotPack.PosY = statePack.aimTargetPos.y;

        InputPack inputPack = new InputPack();
        inputPack.XInput = statePack.input.moveInput.x;
        inputPack.YInput = statePack.input.moveInput.y;
        inputPack.Jump = statePack.input.jump;
        inputPack.FireSeq = statePack.input.fireSeq;
        inputPack.IsFiring = statePack.input.isFiring;

        PlayerPack playerPack = new PlayerPack();
        playerPack.PlayerId = facade.LocalPlayerId;
        playerPack.PlayerStatePack = playerStatePack;
        playerPack.GunRotPack = gunRotPack;
        playerPack.InputPack = inputPack;

        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.PlayerPack.Add(playerPack);

        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        PlayerPack playerPack = pack.PlayerPack[0];

        long playerId = playerPack.PlayerId;

        Input input = new Input();
        ToInput(input, playerPack.InputPack);

        StatePack statePack = new StatePack();
        ToStatePack(statePack, playerPack);
        statePack.input = input;

        mainContext.Post(_ => facade.UpdateCharacterState(playerId, statePack), null);
    }

    private void ToInput(Input input, InputPack inputPack)
    {
        Vector2 moveInput = new Vector2(inputPack.XInput, inputPack.YInput);
        input.moveInput = moveInput;
        input.jump = inputPack.Jump;
        input.isFiring = inputPack.IsFiring;
    }

    private void ToStatePack(StatePack statePack, PlayerPack playerPack)
    {
        statePack.playerPos = new Vector2(playerPack.PlayerStatePack.PosX, playerPack.PlayerStatePack.PosY);
        statePack.velocity = new Vector2(playerPack.PlayerStatePack.XVelocity, playerPack.PlayerStatePack.YVelocity);
        statePack.aimTargetPos = new Vector2(playerPack.GunRotPack.PosX, playerPack.GunRotPack.PosY);
        statePack.animeName = playerPack.PlayerStatePack.AnimeName;
        statePack.isFlip = playerPack.PlayerStatePack.IsFlip;
    }
}
