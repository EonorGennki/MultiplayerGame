using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName ="Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    [Range(0, 1)]
    public float airControl;

    [Header("Collision detected")]
    public float groundCheckDistance;
    public LayerMask whatIsGroud;
}
