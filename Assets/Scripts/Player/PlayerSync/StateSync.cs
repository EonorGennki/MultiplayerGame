using UnityEngine;

public class StateSync : MonoBehaviour
{
    private Transform aimTarget;

    private UpdateCharacterStateRequest stateSyncRequest;
    private Rigidbody2D rb;

    private readonly float sendInterval = .1f; //发送间隔
    private readonly float sendDistanceThreshold = .02f; //距离阈值
    private readonly float angleSendThreshold = .1f; //角度阈值

    private float timer = 0; //计时器
    private Vector2 lastSentPos; //上一次发送的玩家位置
    private float lastSendAngle; //上次发送的角度
    private int lastFireSeq = 0; //上一次开火的序列号
    private Input input; //玩家指令

    public string AnimeName {  get; set; }
    public bool isFlip { get; set; } = false;

    private void Start()
    {
        aimTarget = transform.GetChild(2);

        stateSyncRequest = GetComponent<UpdateCharacterStateRequest>();
        rb = GetComponent<Rigidbody2D>();

        lastSentPos = transform.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > sendInterval)
        {
            timer = 0f;
            TrySend();
        }
    }

    public void SetInput(Input input)
    {
        this.input = input;
    }

    private void TrySend()
    {
        Vector2 playerPos = transform.position;
        float distanceMoved = Vector2.Distance(playerPos, lastSentPos);

        float currentAngle = CalculateGunAngle();
        float angleDiff = Mathf.DeltaAngle(lastSendAngle, currentAngle);

        bool hasMoved = distanceMoved > sendDistanceThreshold;
        bool hasAction = input.jump || input.isFiring || isFlip || input.fireSeq > lastFireSeq;
        bool hasAngleChanged =  Mathf.Abs(angleDiff) > angleSendThreshold;

        bool canSend = hasMoved || hasAction || hasAngleChanged;

        if (!canSend)
        {
            return;
        }

        Send(playerPos);
        lastSentPos = playerPos;
        lastSendAngle = currentAngle;
        lastFireSeq = input.fireSeq;
    }

    private void Send(Vector2 playerPos)
    {
        var statePack = new StatePack
        {
            input = input,

            playerPos = playerPos,
            aimTargetPos = aimTarget.position,
            velocity = rb.velocity,

            animeName = AnimeName,
            isFlip = isFlip
        };

        if (isFlip)
        {
            isFlip = !isFlip;
        }

        stateSyncRequest.SendRequest(statePack);
    }

    private float CalculateGunAngle()
    {
        Vector2 direction = (Vector2)aimTarget.position - (Vector2)transform.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
