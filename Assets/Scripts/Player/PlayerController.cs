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
    private UpdateCharacterStateRequest updateCharacterStateRequest;
    #endregion

    #region State
    private StateMachine stateMachine;
    public PlayerStates PlayerStates { get; private set; }
    #endregion

    #region Input
    private PlayerInputSet playerInput;
    public Input Input { get; private set; }
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
        playerInput = new PlayerInputSet();
        stateMachine = new StateMachine();
        PlayerStates = new PlayerStates(this, stateMachine);
        Input = new Input();
    }

    private void OnDisable()
    {
        if (isLocal)
        {
            Unsubcrise();

            playerInput.Player.Disable();
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

        if (!Input.isFiring || GunController.currentGunData is null)
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

        updateCharacterStateRequest = gameObject.AddComponent<UpdateCharacterStateRequest>();
        playerInput.Player.Enable();

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

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Input.moveInput = ctx.ReadValue<Vector2>();
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        Input.moveInput = Vector2.zero;
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        Input.jump = true;
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        Input.jump = false;
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
    }

    private void OnFirePerformed(InputAction.CallbackContext ctx)
    {
        Input.isFiring = true;
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
    }

    private void OnFireCanceled(InputAction.CallbackContext ctx)
    {
        Input.isFiring = false;
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
    }
    private void OnLeavePerformed(InputAction.CallbackContext ctx) => facade.ShowLeaveGamePanel();

    private void OnAimTargetUpdate(InputAction.CallbackContext ctx)
    {
        aimTarget.position = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        HandleFlip(aimTarget.position);
        updateCharacterStateRequest.SendRequest(Input, aimTarget.transform.position);
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

    private void OnDestroy()
    {
        Unsubcrise();
    }
}
