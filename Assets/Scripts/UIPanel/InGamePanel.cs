using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    [Header("玩家列表")]
    [SerializeField] private GameObject playerStateItem;
    [SerializeField] private Transform playerListTransform;
    private Dictionary<string, PlayerStateItem> playerStateItemDic = new Dictionary<string, PlayerStateItem>();

    [Header("倒计时")]
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private float timer;

    [Header("退出游戏")] 
    [SerializeField] private GameObject exitGame;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        countDown.text = timer.ToString();
    }

    private void OnCancelBtnClick()
    {

    }

    private void OnConfirmBtnClick()
    {
        
    }

    public void UpdateList(List<PlayerInfo> playerList)
    {
        foreach (var player in playerList)
        {
            GameObject o = Instantiate(playerStateItem, Vector3.zero, Quaternion.identity);
            o.transform.SetParent(playerListTransform);
            PlayerStateItem item = o.GetComponent<PlayerStateItem>();
            item.UpdateState(player.PlayerName, player.Health);
            playerStateItemDic.Add(player.PlayerName, item);
        }
    }

    private void OnHealthChanged(string playerId, int health)
    {
        if (playerStateItemDic.TryGetValue(playerId, out PlayerStateItem item))
        {
            item.UpdateHealth(health);
        }
    }
        

    private void AddListeners()
    {
        cancelBtn.onClick.AddListener(OnCancelBtnClick);
        confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        uiManager.OnPlayerListUpdate += UpdateList;
    }

    private void RemoveListers()
    {
        cancelBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
        uiManager.OnPlayerListUpdate -= UpdateList;
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
