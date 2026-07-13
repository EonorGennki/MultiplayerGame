using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : PlayerState
{
    public GroundState(PlayerController player, StateMachine stateMachine, string animeName) : base(player, stateMachine, animeName)
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

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(playerStates.FallState);
        }

        if (input.jump)
        {
            stateMachine.ChangeState(playerStates.JumpState);
        }
    }
}
