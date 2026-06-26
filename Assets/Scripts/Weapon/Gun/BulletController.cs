using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector2 velocity;
    private float damage;
    private float range;
    private Vector2 spawnPos;
    private LayerMask hitLayerMask;

    public void Init(Vector2 velocity, float damage, float range, Vector2 spawnPos, LayerMask hitLayerMask)
    {
        this.velocity = velocity;
        this.damage = damage;
        this.range = range;
        this.spawnPos = spawnPos;
        this.hitLayerMask = hitLayerMask;
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
            Destroy(gameObject);
            return;
        }

        if (Vector2.Distance(spawnPos, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }
}
