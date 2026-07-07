using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Slider healthBar;

    public Event OnChanged;

    public void Init(string playerName, int health)
    {
        this.playerName.text = playerName;
        healthBar.value = health;
    }

    public void UpdateHealth(int health)
    {
        healthBar.value = health;
    }
}
