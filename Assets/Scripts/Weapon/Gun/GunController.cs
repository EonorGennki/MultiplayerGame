using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Gun data")]
    public GunData currentGunData;

    [Header("Transform")]
    [SerializeField] private Transform firePoint; //开枪位置
    [SerializeField] private Transform gunHolder; //武器载点

    private bool canShoot;
    private float spread;
    private float nextFireTime;
    private GameObject currentGunModel;
    private Mouse mouse;
    private Vector3 aimTarget;

    public System.Action<GunData> OnGunChanged;

    private void Start()
    {
        if (currentGunData is not null)
        {
            EquipGun(currentGunData);
        }
    }

    private void Update()
    {
        UpdateAimDirection();

        if (currentGunData is null)
        {
            return;
        }

        spread = System.Math.Max(currentGunData.baseSpread, spread - currentGunData.spreadRecoverySpeed * Time.deltaTime);

        UpdateGunAim();
    }

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="newGunData"></param>
    public void EquipGun(GunData newGunData)
    {
        if (newGunData is null)
        {
            return;
        }

        currentGunData = newGunData;
        spread = newGunData.baseSpread;
        nextFireTime = 0;

        UpdateGunModel();
    }

    /// <summary>
    /// 更换武器模型
    /// </summary>
    private void UpdateGunModel()
    {
        if (currentGunModel is not null)
        {
            Destroy(currentGunModel);
            currentGunModel = null;
        }

        if (currentGunData.gunPrefab is not null && gunHolder is not null)
        {
            currentGunModel = Instantiate(
                currentGunData.gunPrefab,
                gunHolder.position,
                gunHolder.rotation,
                gunHolder
                );
        }
    }

    /// <summary>
    /// 更新武器朝向
    /// </summary>
    private void UpdateGunAim()
    {
        Vector2 aimDirection = (aimTarget - gunHolder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        gunHolder.rotation = Quaternion.Euler(0, 0, angle);

        if (angle > 90 || angle < -90)
        {
            gunHolder.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            gunHolder.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    /// 更新瞄准方向
    /// </summary>
    private void UpdateAimDirection()
    {
        mouse = Mouse.current;

        if (mouse is null)
        {
            return;
        }

        //获取鼠标世界位置
        Vector2 mouseScreenPos = mouse.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        aimTarget = mouseWorldPos;
    }

    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <returns></returns>
    public bool TryShoot()
    {
        if (!canShoot)
        {
            return false;
        }

        ExecuteShoot();
        return true;
    }

    /// <summary>
    /// 执行射击
    /// </summary>
    private void ExecuteShoot()
    {
        nextFireTime = Time.time + currentGunData.fireRate;

        for (int i = 0; i < currentGunData.bulletsPerShot; i++)
        {
            float spreadAngle = Random.Range(-spread, spread);
            Vector2 direction = GetShootDirection(spreadAngle);
        }

        spread = Mathf.Min(spread + currentGunData.spreadIncreasePerShot, currentGunData.maxSpread);
    }

    /// <summary>
    /// 获取射击方向
    /// </summary>
    /// <param name="spreadAngle"></param>
    /// <returns></returns>
    private Vector2 GetShootDirection(float spreadAngle)
    {
        Vector2 shootDirection = (aimTarget - firePoint.position).normalized;

        if (Mathf.Approximately(spreadAngle, 0))
        {
            return shootDirection;
        }

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        angle += spreadAngle;

        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
            );
    }

    private void ProjectileShoot(Vector2 direction)
    {
        if (currentGunData.bulletPrefab == null) return;

        GameObject bullet = Instantiate(
            currentGunData.bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        if (bulletScript != null)
        {
            bulletScript.Init(
                direction * currentGunData.bulletSpeed,
                currentGunData.attack,
                currentGunData.range,
                firePoint.position,
                currentGunData.hitLayerMask
            );
        }
    }

}
