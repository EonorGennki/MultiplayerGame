using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region component
    public Rigidbody2D Rb {  get; private set; }
    public Animator Animator {  get; private set; }
    public GunController GunController { get; private set; }
    #endregion

    private StateMachine stateMachine;
    public PlayerStates PlayerStates { get; private set; }

    #region input
    private PlayerInputSet input;
    public Vector2 MoveInput {  get; private set; }
    public bool Jump {  get; private set; }
    public bool IsFiring { get; private set; }
    #endregion

    [Header("Movement")]
    public float moveSpeed;
    public int jumpForce;
    [Range(0, 1)]
    public float airControl = 1f;
    private bool facingRignt = true;

    [Header("Collision detected")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGroud;
    public bool isGrounded {  get; private set; }

    private void Awake()
    {
        input = new PlayerInputSet();
        stateMachine = new StateMachine();
        PlayerStates = new PlayerStates(this, stateMachine);
    }

    private void OnEnable()
    {
        input.Player.Enable();

        input.Player.Move.performed += OnMovePerformed;
        input.Player.Move.canceled += OnMoveCanceled;

        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;

        input.Player.Fire.performed += OnFirePerformed;
        input.Player.Fire.performed += OnFire;
        input.Player.Fire.canceled += OnFireCanceled;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMovePerformed;
        input.Player.Move.canceled -= OnMoveCanceled;

        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCanceled;

        input.Player.Fire.performed -= OnFirePerformed;
        input.Player.Fire.performed -= OnFire;
        input.Player.Fire.canceled -= OnFireCanceled;

        input.Player.Disable();
    }

    void Start()
    {
        Rb = GetComponentInChildren<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        GunController = GetComponentInChildren<GunController>();
        stateMachine.Initialize(PlayerStates.IdleState);
    }

    void Update()
    {
        stateMachine.UpdateCurrentState();
        DetectCollision();

        if (!IsFiring || GunController.currentGunData is null)
        {
            return;
        }

        //全自动射击
        if (GunController.currentGunData.fireMode == FireMode.FullAuto)
        {
            GunController.TryShoot();
        }
    }

    #region movement

    public void SetVelocity(float xVelocity, float yVeclocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVeclocity);
        HandleFlip(xVelocity);
    }

    public void SetZeroVelocity() => Rb.velocity = Vector2.zero;

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && !facingRignt)
        {
            Flip();
        }
        else if (xVelocity <0 && facingRignt)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRignt = !facingRignt;
    }
    #endregion

    #region Collision detection
    private void DetectCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGroud);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }
    #endregion

    #region Fire
    /// <summary>
    /// 半自动开火
    /// </summary>
    /// <param name="ctx"></param>
    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || GunController.currentGunData is null)
        {
            return;
        }

        if (GunController.currentGunData.fireMode == FireMode.SemiAuto)
        {
            GunController.TryShoot();
        }
    }
    #endregion

    #region Input
    private void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector2.zero;
    private void OnJumpPerformed(InputAction.CallbackContext ctx) => Jump = true;
    private void OnJumpCanceled(InputAction.CallbackContext ctx) => Jump = false;
    private void OnFirePerformed(InputAction.CallbackContext ctx) => IsFiring = true;
    private void OnFireCanceled(InputAction.CallbackContext ctx) => IsFiring = false;
    #endregion
}
