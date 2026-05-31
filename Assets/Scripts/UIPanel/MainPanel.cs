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

        Display();
    }

    public override void OnExit()
    {
        base.OnExit();

        Hide();
    }

    private void Display()
    {
        gameObject.SetActive(true);
        AddListeners();
    }

    private void Hide()
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

        Display();
    }
}
