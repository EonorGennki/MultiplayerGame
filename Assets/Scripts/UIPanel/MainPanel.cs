using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    [SerializeField] private Button startBtn;

    private void OnStartBtnClick()
    {
        uiManager.PushPanel(PanelType.Login);
    }
    private void AddListeners()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
    }

    private void RemoveListeners()
    {
        startBtn.onClick.RemoveAllListeners();
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

    protected override void Show()
    {
        gameObject.SetActive(true);
        AddListeners();
    }

    protected override void Hide()
    {
        RemoveListeners();
        gameObject.SetActive(false);
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
