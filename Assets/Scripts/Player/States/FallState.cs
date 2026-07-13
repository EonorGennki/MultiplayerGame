public class FallState : AirState
{
    public FallState(PlayerController player, StateMachine stateMachine, string animeName) : base(player, stateMachine, animeName)
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

        if (player.IsGrounded)
        {
            stateMachine.ChangeState(playerStates.IdleState);
        }
    }
}
