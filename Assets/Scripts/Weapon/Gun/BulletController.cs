using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Bullet stats
    private Vector2 velocity;
    private float damage;
    private float range;
    private Vector2 spawnPos;
    private LayerMask hitLayerMask;
    #endregion

    private float lifeTimer = 5f;

    private ObjectPool<BulletController> pool;

    private void Update()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            velocity.normalized,
            velocity.magnitude * Time.deltaTime + 0.1f,
            hitLayerMask
        );

        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
            ReturnBullet();
            return;
        }

        if (Vector2.Distance(spawnPos, transform.position) > range)
        {
            ReturnBullet();
        }

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            ReturnBullet();
            lifeTimer = 5f;
        }
    }

    /// <summary>
    ///子弹初始化
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <param name="velocity"></param>
    /// <param name="damage"></param>
    /// <param name="range"></param>
    /// <param name="hitLayerMask"></param>
    /// <param name="pool"></param>
    public void Init(Vector2 spawnPos, Vector2 velocity, float damage, float range,  LayerMask hitLayerMask, ObjectPool<BulletController> pool)
    {
        Debug.Log(spawnPos);
        Debug.Log(velocity);
        Debug.Log(damage);
        Debug.Log(range);
        Debug.Log(hitLayerMask);
        Debug.Log(pool);
        this.spawnPos = spawnPos;
        this.velocity = velocity;
        this.damage = damage;
        this.range = range;
        this.hitLayerMask = hitLayerMask;
        this.pool = pool;
        transform.right = velocity.normalized;
    }

    /// <summary>
    /// 回收子弹
    /// </summary>
    public void ReturnBullet()
    {
        BulletPool.Instance.ReturnBullet(this, pool);
    }
}
