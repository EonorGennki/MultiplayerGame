using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameFacade facade;

    #region Component
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public GunController GunController { get; private set; }
    #endregion

    #region State
    private StateMachine stateMachine;
    public PlayerStates PlayerStates { get; private set; }
    #endregion

    #region Input
    private PlayerInputSet input;
    public Vector2 MoveInput { get; private set; }
    public bool Jump { get; private set; }
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
    public bool isGrounded { get; private set; }

    private Transform aimTarget;

    private bool isLocal = false;

    private void Awake()
    {
        input = new PlayerInputSet();
        stateMachine = new StateMachine();
        PlayerStates = new PlayerStates(this, stateMachine);
    }

    private void OnDisable()
    {
        if (isLocal)
        {
            Unsubcrise();

            input.Player.Disable();
        }
    }

    void Start()
    {
        facade = GameFacade.Instance;
        Rb = GetComponentInChildren<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        GunController = GetComponentInChildren<GunController>();
        stateMachine.Initialize(PlayerStates.IdleState);
        aimTarget = GetComponentsInChildren<Transform>().Last();
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

    public void Init(bool isLocal)
    {
        this.isLocal = isLocal;

        if (!this.isLocal)
        {
            return;
        }

        input.Player.Enable();

        Subscribe();
    }

    #region movement
    public void SetVelocity(float xVelocity, float yVeclocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVeclocity);
    }

    public void SetZeroVelocity() => Rb.velocity = Vector2.zero;

    private void HandleFlip(Vector2 mousePos)
    {
        float direction = mousePos.x - transform.position.x;

        //防抖处理
        if (Mathf.Abs(direction) < .1f)
        {
            return;
        }

        if (mousePos.x < transform.position.x && facingRignt)
        {
            Flip();
        }
        else if (mousePos.x > transform.position.x && !facingRignt)
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
    /// <summary>
    /// 订阅输入事件
    /// </summary>
    private void Subscribe()
    {
        input.Player.Move.performed += OnMovePerformed;
        input.Player.Move.canceled += OnMoveCanceled;

        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;

        input.Player.Aim.performed += OnAimTargetUpdate;

        input.Player.Fire.performed += OnFirePerformed;
        input.Player.Fire.performed += OnFire;
        input.Player.Fire.canceled += OnFireCanceled;

        input.Player.Leave.performed += OnLeavePerformed;
    }

    /// <summary>
    /// 取消订阅输入事件
    /// </summary>
    private void Unsubcrise()
    {
        input.Player.Move.performed -= OnMovePerformed;
        input.Player.Move.canceled -= OnMoveCanceled;

        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCanceled;

        input.Player.Aim.performed -= OnAimTargetUpdate;

        input.Player.Fire.performed -= OnFirePerformed;
        input.Player.Fire.performed -= OnFire;
        input.Player.Fire.canceled -= OnFireCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector2.zero;
    private void OnJumpPerformed(InputAction.CallbackContext ctx) => Jump = true;
    private void OnJumpCanceled(InputAction.CallbackContext ctx) => Jump = false;
    private void OnFirePerformed(InputAction.CallbackContext ctx) => IsFiring = true;
    private void OnFireCanceled(InputAction.CallbackContext ctx) => IsFiring = false;
    private void OnLeavePerformed(InputAction.CallbackContext ctx) => facade.ShowLeaveGamePanel();

    private void OnAimTargetUpdate(InputAction.CallbackContext ctx)
    {
        aimTarget.position = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        HandleFlip(aimTarget.position);
    }
    #endregion

    private void OnDestroy()
    {
        Unsubcrise();
    }
}
