using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanel : BasePanel
{
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Show();
    }

    public override void OnExit()
    {
        base.OnExit();
        Hide();
    }

    public override void OnPause()
    {
        base.OnPause();
        Hide();
    }

    public override void OnResume()
    {
        base.OnResume();
        Show();
    }
}
