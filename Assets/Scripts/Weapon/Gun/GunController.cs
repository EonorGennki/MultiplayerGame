using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Gun data")]
    public GunData currentGunData;

    [Header("Transform")]
    [SerializeField] private Transform firePoint; //开枪位置
    [SerializeField] private Transform gunHolder; //武器载点
    [SerializeField] private Transform aimTarget;

    private bool canShoot => Time.time >= nextFireTime && currentGunData is not null;
    private float spread;
    private float nextFireTime;
    private GameObject currentGunModel;
    private Mouse mouse;

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

            firePoint = currentGunModel.transform.GetChild(0);
        }
    }

    /// <summary>
    /// 更新武器朝向
    /// </summary>
    private void UpdateGunAim()
    {
        Vector2 aimDirection = (aimTarget.position - gunHolder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        bool ShouldFlip = aimTarget.position.x - transform.position.x < 0 ? true : false;

        if (!ShouldFlip)
        {
            if (angle > 60)
            {
                angle = 60;
            }
            else if (angle < -70)
            {
                angle = -70;
            }

            gunHolder.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (angle > 0 && angle < 120)
            {
                angle = 120;
            }
            else if (angle < 0 && angle > -110)
            {
                angle = -110;
            }

            gunHolder.localScale = new Vector3(1, -1, 1);
        }

        gunHolder.rotation = Quaternion.Euler(0, 0, angle);
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
            ShootBullet(direction);
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
        Vector2 shootDirection = (aimTarget.position - firePoint.position).normalized;

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

    private void ShootBullet(Vector2 direction)
    {
        if (currentGunData.bulletPrefab is null) return;

        BulletPool.Instance.ShootBullet(currentGunData, firePoint.position, direction);
    }

}
