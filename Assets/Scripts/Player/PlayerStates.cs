using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates
{
    public IdleState IdleState {  get; private set; }
    public MoveState MoveState {  get; private set; }
    public JumpState JumpState {  get; private set; }
    public FallState FallState { get; private set; }

    public PlayerStates(PlayerController player, StateMachine stateMachine)
    {
        IdleState = new IdleState(player, stateMachine, "idle");
        MoveState = new MoveState(player, stateMachine, "move");
        JumpState = new JumpState(player, stateMachine, "jump");
        FallState = new FallState(player, stateMachine, "jump");
    }
}
