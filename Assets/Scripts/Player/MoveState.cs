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

        if (player.MoveInput.x == 0)
        {
            stateMachine.ChangeState(playerStates.IdleState);
        }

        player.SetVelocity(player.MoveInput.x * player.moveSpeed, rb.velocity.y);
    }
}
