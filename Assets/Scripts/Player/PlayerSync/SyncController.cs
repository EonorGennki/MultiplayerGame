using UnityEngine;

public class SyncController : MonoBehaviour
{
    private readonly float syncSpeed = 15f;
    private readonly float teleportThreshold = 2f; //瞬移阈值

    private Animator Animator;
    private GunController GunController;
    private Collider2D col;
    private PlayerData playerData;

    private Vector2 targetPos;
    private Vector2 targetVelocity;
    private readonly float smoothTime = .1f;
    private Transform aimTarget;
    private string OldAnimeName = "idle";

    private float lastUpdateTime; //上一次更新状态时间
    private int lastFireSeq = 0; //上一次单发开火序列号
    private bool facingRight = true;
    private bool hasNewData;

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        GunController = GetComponent<GunController>();
        col = GetComponent<Collider2D>();
        playerData = Resources.Load<PlayerData>("PlayerData/Player");

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        aimTarget = transform.GetChild(2);
        Animator.SetBool(OldAnimeName, true);
    }

    private void Update()
    {
        SyncPos();

        Animator.SetFloat("yVelocity", targetVelocity.y);
    }

    /// <summary>
    /// 同步位置
    /// </summary>
    private void SyncPos()
    {
        Vector2 predictedPos = targetPos;

        //接收到新数据时预测目标位置
        if (hasNewData)
        {
            float timeInterval = Mathf.Min(Time.time - lastUpdateTime, .2f);
            predictedPos = targetPos + targetVelocity * timeInterval;
            predictedPos = ClampToGround(predictedPos);
            hasNewData = false;
        }

        float distance = Vector2.Distance(transform.position, targetPos);
        float currentSmoothTime = smoothTime;

        //距离太远或太小直接跳变
        if (distance > teleportThreshold || distance < 0.01f)
        {
            transform.position = targetPos;
            return;
        }

        //动态调整平滑时间
        if (distance > 1f)
        {
            currentSmoothTime = smoothTime * .5f;
        }
        else if (distance < .1f)
        {
            currentSmoothTime = smoothTime * 1.5f;
        }

        //计算插值因子
        float t = 1f - Mathf.Exp(-syncSpeed * Time.deltaTime);
        //追赶目标位置
        Vector2 newPos = Vector2.Lerp(
                transform.position,
                predictedPos,
                t
                );


        distance = Vector2.Distance(newPos, predictedPos);

        //距离逼近阈值直接对齐
        if (distance < .01f)
        {
            transform.position = predictedPos;
        }
        else
        {
            transform.position = newPos;
        }
    }

    /// <summary>
    /// 状态同步
    /// </summary>
    /// <param name="statePack"></param>
    public void Sync(StatePack statePack)
    {
        targetPos = statePack.playerPos;
        targetVelocity = statePack.velocity;
        lastUpdateTime = Time.time;
        aimTarget.position = statePack.aimTargetPos;
        hasNewData = true;

        PlayAnime(statePack.animeName);
        Shoot(statePack.input);

        if (statePack.isFlip)
        {
            Flip();
            facingRight = !facingRight;
            GunController.SetShouldScale(!facingRight);
        }
    }

    /// <summary>
    /// 射击
    /// </summary>
    /// <param name="statePack"></param>
    private void Shoot(Input input)
    {

        if (GunController.CurrentGunData.fireMode == FireMode.FullAuto)
        {
            if (input.isFiring)
            {
                GunController.TryShoot();
            }
        }

        if (GunController.CurrentGunData.fireMode == FireMode.SemiAuto)
        {
            if (input.fireSeq > lastFireSeq)
            {
                GunController.TryShoot();
                lastFireSeq = input.fireSeq;
            }
        }
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animeName"></param>
    public void PlayAnime(string animeName)
    {
        if (OldAnimeName == animeName)
        {
            return;
        }

        Animator.SetBool(OldAnimeName, false);
        Animator.SetBool(animeName, true);
        OldAnimeName = animeName;
    }


    /// <summary>
    /// 翻转
    /// </summary>
    private void Flip() => transform.Rotate(0, 180, 0);

    /// <summary>
    /// 预测位置不得低于地面
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector2 ClampToGround(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, playerData.groundCheckDistance, playerData.whatIsGroud);

        if (hit.collider is null)
        {
            return pos;
        }

        float groundY = hit.point.y + col.bounds.extents.y;
        if (pos.y < groundY)
        {
            pos.y = groundY;
        }
        return pos;
    }

}
