using SocketGameProtocal;
using System.Collections;
using System.Collections.Generic;
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

    public void SendRequest(Input input, Vector2 pos)
    {
        MainPack pack = new MainPack();
        PlayerPack playerPack = new PlayerPack();
        GunRotPack posPack = new GunRotPack();
        InputPack inputPack = new InputPack();

        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        
        playerPack.PlayerId = facade.LocalPlayerId;

        posPack.PosX = pos.x;
        posPack.PosY = pos.y;

        inputPack.XInput = input.moveInput.x;
        inputPack.YInput = input.moveInput.y;
        inputPack.Jump = input.jump;
        inputPack.IsFiring = input.isFiring;

        playerPack.GunRotPack = posPack;
        playerPack.InputPack = inputPack;
        pack.PlayerPack.Add(playerPack);

        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        PlayerPack playerPack = pack.PlayerPack[0];
        long playerId = playerPack.PlayerId;
        Vector2 aimTargetPos = new Vector2(playerPack.GunRotPack.PosX, playerPack.GunRotPack.PosY);
        Input input = new Input();
        input = ToInput(input, playerPack.InputPack);

        mainContext.Post(_ => facade.UpdateCharacterState(playerId, input, aimTargetPos), null);
    }

    private Input ToInput(Input input, InputPack inputPack)
    {
        Vector2 moveInput = new Vector2(inputPack.XInput, inputPack.YInput);
        input.moveInput = moveInput;
        input.jump = inputPack.Jump;
        input.isFiring = inputPack.IsFiring;
        return input;
    }
}
