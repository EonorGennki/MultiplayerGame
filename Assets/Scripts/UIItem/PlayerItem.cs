using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    public void SetPlayerInfo(PlayerInfo playerInfo)
    {
        this.playerName.text = playerInfo.playerName;
    }
}
