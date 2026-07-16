using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Slider healthBar;
    

    public void Init(string playerName, int health)
    {
        this.playerName.text = playerName;
        healthBar.maxValue = health;
        healthBar.minValue = 0;
        healthBar.value = health;
        score.text = 0.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthBar.value = health;
    }

    public void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }
}
