using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Slider slider;

    public Event OnChanged;

    public void Init(string playername, int value)
    {
        playerName.text = playername;
        slider.value = value;
    }


}
