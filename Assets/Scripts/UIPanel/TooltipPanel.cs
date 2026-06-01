using TMPro;
using UnityEngine;

public class TooltipPanel : BasePanel
{
    public TextMeshProUGUI text;
    private string msg;

    private void Update()
    {
        if (msg is not null)
        {
            ShowText(msg);
            msg = null;
        }
    }

    public void Show(string str, bool sync = false)
    {
        if (sync)
        {
            Debug.Log(sync);
            msg = str;
        }

        gameObject.SetActive(true);
        ShowText(str);
    }

    private void ShowText(string str)
    {
        text.text = str;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
