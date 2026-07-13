using UnityEngine;

public class IdleState : GroundState
{
    public IdleState(PlayerController player, StateMachine stateMachine, string animeName) : base(player, stateMachine, animeName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.velocity.y);
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
            stateMachine.ChangeState(playerStates.MoveState);
        }
    }
}
