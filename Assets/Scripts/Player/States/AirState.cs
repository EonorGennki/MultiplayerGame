using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerState
{
    public AirState(PlayerController player, StateMachine stateMachine, string animeName) : base(player, stateMachine, animeName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (input.moveInput.x != 0)
        {
            player.SetVelocity(input.moveInput.x * player.MoveSpeed * player.AirControl, rb.velocity.y);
        }
    }
}
