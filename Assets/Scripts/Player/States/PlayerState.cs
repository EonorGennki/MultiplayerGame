using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected StateMachine stateMachine;
    protected string animeName;

    protected PlayerStates playerStates;
    protected Animator animator;
    protected Rigidbody2D rb;

    public PlayerState(PlayerController player, StateMachine stateMachine, string animeName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animeName = animeName;
    }

    public virtual void Enter()
    {
        playerStates = player.PlayerStates;
        animator = player.Animator;
        rb = player.Rb;

        animator.SetBool(animeName, true);
    }

    public virtual void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        animator.SetBool(animeName, false);
    }
}
