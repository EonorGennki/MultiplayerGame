using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    [Header("玩家列表")]
    [SerializeField] private GameObject playerStateItem;
    [SerializeField] private Transform playerListTransform;

    [Header("倒计时")]
    [SerializeField] private TextMeshProUGUI countDown;

    [Header("退出游戏")] 
    [SerializeField] private GameObject exitGame;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    private void OnCancelBtnClick()
    {

    }

    private void OnConfirmBtnClick()
    {
        
    }

    private void AddListeners()
    {
        cancelBtn.onClick.AddListener(OnCancelBtnClick);
        confirmBtn.onClick.AddListener(OnConfirmBtnClick);
    }

    private void RemoveListers()
    {
        cancelBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }
}
