using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private long playerId;
    private EventCenter eventCenter;
    private UpdateHealthRequest updateHealthRequest;

    public int MaxHealth { get; private set; }

    private void Start()
    {
        playerId = GameFacade.Instance.LocalPlayerId;
        eventCenter = GameFacade.Instance.EventCenter;
        updateHealthRequest = GetComponent<UpdateHealthRequest>();
    }

    /// <summary>
    /// 重置生命值
    /// </summary>
    public void ResetHealth()
    {
        updateHealthRequest.SendRequest(playerId, MaxHealth);
    }

    public void SetMaxHealth(int maxHealth) => MaxHealth = maxHealth;

    public void TakeDamage(long attackPlayerId, int damage)
    {
        updateHealthRequest.SendRequest(playerId, -damage, attackPlayerId);
    }

    /// <summary>
    /// 更新生命值
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="health"></param>
    /// <param name="isDead"></param>
    public void UpdateHealth(long playerId, int health, bool isDead)
    {
        eventCenter.TriggerHealthChanged(playerId, health);

        if (playerId == GameFacade.Instance.LocalPlayerId)
        {
            if (isDead)
            {
                GameFacade.Instance.Respawn();
            }
        }
    }
}
