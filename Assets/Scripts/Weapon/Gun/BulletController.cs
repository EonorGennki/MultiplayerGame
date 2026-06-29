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

    public void Init(Vector2 spawnPos, Vector2 velocity, float damage, float range,  LayerMask hitLayerMask, ObjectPool<BulletController> pool)
    {
        this.spawnPos = spawnPos;
        this.velocity = velocity;
        this.damage = damage;
        this.range = range;
        this.hitLayerMask = hitLayerMask;
        this.pool = pool;
        transform.right = velocity.normalized;
    }

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
            damageable?.TakeDamage(damage, hit.point, velocity.normalized);
            ReturnBullet();
            return;
        }

        if (Vector2.Distance(spawnPos, transform.position) >= range)
        {
            ReturnBullet();
        }

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            ReturnBullet();
        }
    }

    /// <summary>
    /// »ØÊÕ×Óµ¯
    /// </summary>
    public void ReturnBullet()
    {
        BulletPool.Instance.ReturnBullet(this, pool);
    }
}
