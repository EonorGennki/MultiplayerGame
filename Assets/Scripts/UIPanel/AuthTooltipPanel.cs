using TMPro;
using UnityEngine;

public class AuthTooltipPanel : BasePanel, ITooltipPanel
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

    protected override void Hide()
    {
        text.text = "";
        gameObject.SetActive (false);
    }
}
