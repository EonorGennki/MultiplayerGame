using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [Header("子弹预制体配置")]
    [SerializeField] private BulletPrefabConfig[] bulletConfigs;

    [Header("池配置")]
    [SerializeField] private int initialPoolSize = 30;
    [SerializeField] private int maxPoolSize = 100;
    [SerializeField] private Transform poolParent;

    // 存储所有子弹池（用预制体作为Key）
    private Dictionary<GameObject, ObjectPool<BulletController>> bulletPools = new Dictionary<GameObject, ObjectPool<BulletController>>();

    // 活跃子弹列表（用于管理）
    private List<BulletController> activeBullets = new List<BulletController>();

    public static BulletPool Instance { get; private set; }

    [System.Serializable]
    public class BulletPrefabConfig
    {
        public BulletController bulletPrefab;       // 子弹预制体
        public int initialSize = 20;        // 该类型子弹的初始池大小
        public int maxSize = 50;            // 该类型子弹的最大池大小
    }

    private void Awake()
    {
        Instance = this;

        if (poolParent is null)
            poolParent = transform;

        // 为每种子弹创建独立池
        foreach (var config in bulletConfigs)
        {
            if (config.bulletPrefab is null) continue;

            var pool = new ObjectPool<BulletController>(
                config.bulletPrefab,
                poolParent,
                config.initialSize > 0 ? config.initialSize : initialPoolSize,
                config.maxSize > 0 ? config.maxSize : maxPoolSize
            );

            bulletPools[config.bulletPrefab.gameObject] = pool;
        }
    }

    /// <summary>
    /// 发射子弹（通过预制体）
    /// </summary>
    public void ShootBullet(GameObject bulletPrefab, Vector2 firePoint, Vector2 velocity,
                            int damage, float range, LayerMask hitLayerMask)
    {
        if (bulletPrefab is null) return;

        // 获取对应的池
        if (!bulletPools.TryGetValue(bulletPrefab, out ObjectPool<BulletController> pool))
        {
            return;
        }

        // 从池获取子弹
        BulletController bullet = pool.Get();
        bullet.transform.position = firePoint;
        bullet.Init(firePoint, velocity, damage, range, hitLayerMask, pool);

        activeBullets.Add(bullet);
    }

    /// <summary>
    /// 发射子弹（通过武器数据中的预制体）
    /// </summary>
    public void ShootBullet(GunData gunData, Vector2 firePoint, Vector2 direction)
    {
        if (gunData is null ) return;

        ShootBullet(
            gunData.bulletPrefab,
            firePoint,
            direction * gunData.bulletSpeed,
            gunData.damage,
            gunData.range,
            gunData.hitLayerMask
        );
    }

    /// <summary>
    /// 回收子弹
    /// </summary>
    public void ReturnBullet(BulletController bullet, ObjectPool<BulletController> pool)
    {
        if (bullet is null) return;

        activeBullets.Remove(bullet);
        pool?.Return(bullet);
    }

    /// <summary>
    /// 回收所有子弹
    /// </summary>
    public void ReturnAllBullets()
    {
        List<BulletController> bulletsToReturn = new List<BulletController>(activeBullets);
        foreach (var bullet in bulletsToReturn)
        {
            bullet.ReturnBullet();
        }
        activeBullets.Clear();
    }
}
