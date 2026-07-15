using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private EventCenter eventCenter;
    private UpdateHealthRequest updateHealthRequest;

    private long playerId;
    public int MaxHealth { get; private set; }
    private int currentHealth;

    private void Start()
    {
        eventCenter = GameFacade.Instance.EventCenter;
        updateHealthRequest = GetComponent<UpdateHealthRequest>();
        playerId = GameFacade.Instance.LocalPlayerId;
        currentHealth = MaxHealth;
    }

    public void SetMaxHealth(int maxHealth) => MaxHealth = maxHealth;

    public void TakeDamage(int damage)
    {
        updateHealthRequest.SendRequest(playerId, damage);
    }

    public void UpdateHealth(long playerId, int health)
    {
        currentHealth = health;
        eventCenter.TriggerHealthChanged(playerId, currentHealth);
    }

}
