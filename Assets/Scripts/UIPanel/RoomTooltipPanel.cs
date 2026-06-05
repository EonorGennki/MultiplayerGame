using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomTooltipPanel : BasePanel, ITooltipPanel
{
    [SerializeField] private TextMeshProUGUI text;

    public void Show(string str)
    {
        gameObject.SetActive(true);

        ShowText(str);

        Invoke(nameof(Hide), 2);
    }

    private void ShowText(string str)
    {
        text.text = str;
    }

    private void Hide()
    {
        text.text = "";
        gameObject.SetActive(false);
    }
}
