using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    private LeaveGameRequest exitGameRequest;

    [Header("玩家列表")]
    [SerializeField] private GameObject playerStateItem;
    [SerializeField] private Transform playerListTransform;
    private Dictionary<string, PlayerStateItem> playerStateItemDic = new Dictionary<string, PlayerStateItem>();

    [Header("倒计时")]
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private float timer;

    [Header("退出游戏")]
    [SerializeField] private GameObject exitGamePanel;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    protected override void Start()
    {
        exitGameRequest = GetComponent<LeaveGameRequest>();
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer< 0)
            {
                timer = 0;
            }

            countDown.text = Mathf.Floor(timer).ToString();
        }
    }

    private void OnCancelBtnClick()
    {
        exitGamePanel.SetActive(false);
    }

    private void OnConfirmBtnClick()
    {
        exitGameRequest.SendRequest();
        exitGamePanel.SetActive(false);
    }

    public void UpdateList(List<PlayerInfo> playerList)
    {
        for (int i = playerListTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerListTransform.GetChild(i).gameObject);
        }

        playerStateItemDic.Clear();

        foreach (var player in playerList)
        {
            GameObject o = Instantiate(playerStateItem, playerListTransform, false);
            PlayerStateItem item = o.GetComponent<PlayerStateItem>();
            item.Init(player.PlayerName, player.Health);
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

    public void ShowLeaveGamePanel() => exitGamePanel.SetActive(true);

    private void AddListeners()
    {
        cancelBtn.onClick.AddListener(OnCancelBtnClick);
        confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        uiManager.OnPlayerListUpdate += UpdateList;
    }

    private void RemoveListeners()
    {
        cancelBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
        uiManager.OnPlayerListUpdate -= UpdateList;
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
