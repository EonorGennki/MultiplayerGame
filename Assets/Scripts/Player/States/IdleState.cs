using UnityEngine;

public class IdleState : GroundState
{
    public IdleState(PlayerController player, StateMachine stateMachine, string animeName) : base(player, stateMachine, animeName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x != 0)
        {
            stateMachine.ChangeState(playerStates.MoveState);
        }
    }
}
