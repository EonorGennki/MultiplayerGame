using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    [SerializeField] private Button startBtn;

    private void OnStartBtnClick()
    {
        Debug.Log("click");
        uiManager.PushPanel(PanelType.SignUp);
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

    private void AddListeners()
    {
        Debug.Log(gameObject.activeSelf);

        startBtn.onClick.AddListener(OnStartBtnClick);
    }

    private void RemoveListeners()
    {
        startBtn.onClick.RemoveAllListeners();
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
}
