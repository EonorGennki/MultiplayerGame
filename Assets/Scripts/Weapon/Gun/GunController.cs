using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    private long playerId;
    public GunData CurrentGunData { get; private set; }

    private Transform firePoint; //开枪位置
    private Transform gunHolder; //武器载点
    private Transform aimTarget; //瞄准位置

    private bool CanShoot => Time.time >= nextFireTime && CurrentGunData is not null; //能否执行射击标识
    private float spread; //散步
    private float nextFireTime; //下次开火时间
    private GameObject currentGunModel; //当前武器模型
    private bool shouldScale; //是否旋转标识

    public System.Action<GunData> OnGunChanged;

    private void Start()
    {
        CurrentGunData = Resources.Load<GunData>("GunData/HG");

        gunHolder = transform.GetChild(0);
        aimTarget = transform.GetChild(2);

        if (CurrentGunData is not null)
        {
            EquipGun(CurrentGunData);
        }
    }

    private void Update()
    {

        if (CurrentGunData is null)
        {
            return;
        }

        spread = System.Math.Max(CurrentGunData.baseSpread, spread - CurrentGunData.spreadRecoverySpeed * Time.deltaTime);

        UpdateGunAim();
    }

    public void SetPlayerId(long playerId) => this.playerId = playerId;

    public void SetShouldScale(bool value) => shouldScale = value;

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

        CurrentGunData = newGunData;
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

        if (CurrentGunData.gunPrefab is not null && gunHolder is not null)
        {
            currentGunModel = Instantiate(
                CurrentGunData.gunPrefab,
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

        if (!shouldScale)
        {
            gunHolder.localScale = new Vector3(1, 1, 1);

            if (angle > 60)
            {
                angle = 60;
            }
            else if (angle < -70)
            {
                angle = -70;
            }
        }
        else
        {
            gunHolder.localScale = new Vector3(1, -1, 1);

            if (angle > 0 && angle < 120)
            {
                angle = 120;
            }
            else if (angle < 0 && angle > -110)
            {
                angle = -110;
            }
        }

        gunHolder.rotation = Quaternion.Euler(0, 0, angle);
    }


    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <returns></returns>
    public bool TryShoot()
    {
        if (!CanShoot)
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
        nextFireTime = Time.time + CurrentGunData.fireRate;

        for (int i = 0; i < CurrentGunData.bulletsPerShot; i++)
        {
            float spreadAngle = Random.Range(-spread, spread);
            Vector2 direction = GetShootDirection(spreadAngle);
            ShootBullet(playerId, direction);
        }

        spread = Mathf.Min(spread + CurrentGunData.spreadIncreasePerShot, CurrentGunData.maxSpread);
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

    /// <summary>
    /// 发射子弹
    /// </summary>
    /// <param name="direction"></param>
    private void ShootBullet(long playerId, Vector2 direction)
    {
        if (CurrentGunData.bulletPrefab is null) return;
        BulletPool.Instance.ShootBullet(playerId, CurrentGunData, firePoint.position, direction);
    }

}
