using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameFacade facade;

    #region Component
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public GunController GunController { get; private set; }
    public StateSync stateSync { get; private set; }
    #endregion

    #region State
    private StateMachine stateMachine;
    public PlayerStates PlayerStates { get; private set; }
    #endregion

    #region Input
    private PlayerInputSet playerInput;
    public Input Input { get; private set; }
    #endregion

    public PlayerData PlayerData { get; private set; }
    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }
    public float AirControl { get; private set; }
    private bool facingRight = true;

    public float GroundCheckDistance { get; private set; }
    public LayerMask WhatIsGround { get; private set; }
    public bool IsGrounded { get; private set; }

    private Transform aimTarget;

    private void Awake()
    {
        playerInput = new PlayerInputSet();
        stateMachine = new StateMachine();
        PlayerStates = new PlayerStates(this, stateMachine);
        Input = new Input();
        PlayerData = Resources.Load<PlayerData>("PlayerData/Player");
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();

        Subscribe();
    }

    private void OnDisable()
    {
        Unsubcrise();

        playerInput.Player.Disable();
    }

    void Start()
    {
        MoveSpeed = PlayerData.moveSpeed;
        JumpForce = PlayerData.jumpForce;
        AirControl = PlayerData.airControl;
        GroundCheckDistance = PlayerData.groundCheckDistance;
        WhatIsGround = PlayerData.whatIsGroud;

        facade = GameFacade.Instance;
        Animator = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        GunController = GetComponent<GunController>();
        stateSync = GetComponent<StateSync>();
        aimTarget = transform.GetChild(2);

        stateMachine.Initialize(PlayerStates.IdleState);

        stateSync.SetInput(Input);
    }

    void Update()
    {
        stateMachine.UpdateCurrentState();
        DetectCollision();



        if (!Input.isFiring || GunController.CurrentGunData is null)
        {
            return;
        }

        //全自动射击
        if (GunController.CurrentGunData.fireMode == FireMode.FullAuto)
        {
            GunController.TryShoot();
        }
    }

    #region movement
    public void SetVelocity(float xVelocity, float yVeclocity)
    {
        Rb.velocity = new Vector2(xVelocity, yVeclocity);
    }

    public void SetZeroVelocity() => Rb.velocity = Vector2.zero;

    /// <summary>
    /// 处理人物翻转
    /// </summary>
    /// <param name="mousePos"></param>
    public void HandleFlip(Vector2 mousePos)
    {
        float direction = mousePos.x - transform.position.x;

        if (Mathf.Abs(direction) < .1f)
        {
            return;
        }

        if (direction < 0 && facingRight)
        {
            Flip();
            GunController.SetShouldScale(true);
        }
        else if (direction > 0 && !facingRight)
        {
            Flip();
            GunController.SetShouldScale(false);
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        stateSync.isFlip = true;
    }
    #endregion

    #region Collision detection
    private void DetectCollision()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, WhatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -GroundCheckDistance));
    }
    #endregion

    #region Fire
    /// <summary>
    /// 半自动开火
    /// </summary>
    /// <param name="ctx"></param>
    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || GunController.CurrentGunData is null)
        {
            return;
        }

        if (GunController.CurrentGunData.fireMode == FireMode.SemiAuto)
        {
            GunController.TryShoot();
            Input.fireSeq++;
        }
    }
    #endregion

    #region Input
    /// <summary>
    /// 订阅输入事件
    /// </summary>
    private void Subscribe()
    {
        playerInput.Player.Move.performed += OnMovePerformed;
        playerInput.Player.Move.canceled += OnMoveCanceled;

        playerInput.Player.Jump.performed += OnJumpPerformed;
        playerInput.Player.Jump.canceled += OnJumpCanceled;

        playerInput.Player.Aim.performed += OnAimTargetUpdate;

        playerInput.Player.Fire.performed += OnFirePerformed;
        playerInput.Player.Fire.performed += OnFire;
        playerInput.Player.Fire.canceled += OnFireCanceled;

        playerInput.Player.Leave.performed += OnLeavePerformed;
    }

    /// <summary>
    /// 取消订阅输入事件
    /// </summary>
    private void Unsubcrise()
    {
        playerInput.Player.Move.performed -= OnMovePerformed;
        playerInput.Player.Move.canceled -= OnMoveCanceled;

        playerInput.Player.Jump.performed -= OnJumpPerformed;
        playerInput.Player.Jump.canceled -= OnJumpCanceled;

        playerInput.Player.Aim.performed -= OnAimTargetUpdate;

        playerInput.Player.Fire.performed -= OnFirePerformed;
        playerInput.Player.Fire.performed -= OnFire;
        playerInput.Player.Fire.canceled -= OnFireCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) => Input.moveInput = ctx.ReadValue<Vector2>();

    private void OnMoveCanceled(InputAction.CallbackContext ctx) => Input.moveInput = Vector2.zero;

    private void OnJumpPerformed(InputAction.CallbackContext ctx) => Input.jump = true;

    private void OnJumpCanceled(InputAction.CallbackContext ctx) => Input.jump = false;

    private void OnFirePerformed(InputAction.CallbackContext ctx) => Input.isFiring = true;

    private void OnFireCanceled(InputAction.CallbackContext ctx) => Input.isFiring = false;
    private void OnLeavePerformed(InputAction.CallbackContext ctx) => facade.ShowLeaveGamePanel();

    private void OnAimTargetUpdate(InputAction.CallbackContext ctx)
    {
        aimTarget.position = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        HandleFlip(aimTarget.position);
    }
    #endregion

    #region Set input and aim target
    public void SetInput(Input input)
    {
        Input.moveInput = input.moveInput;
        Input.jump = input.jump;
        Input.isFiring = input.isFiring;
    }

    public void SetAimTarget(Vector2 aimTargetPos) => aimTarget.transform.position = aimTargetPos;
    #endregion
}
