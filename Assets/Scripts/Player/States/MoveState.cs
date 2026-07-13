public class MoveState : GroundState
{
    public MoveState(PlayerController player, StateMachine stateMachine, string animeName) : base(player, stateMachine, animeName)
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

        if (input.moveInput.x == 0)
        {
            stateMachine.ChangeState(playerStates.IdleState);
        }

        player.SetVelocity(input.moveInput.x * player.MoveSpeed, rb.velocity.y);
    }
}
