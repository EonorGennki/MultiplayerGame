using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    private LeaveGameRequest exitGameRequest;
    private GameFacade facade;

    [Header("玩家列表")]
    [SerializeField] private GameObject playerStatsItem;
    [SerializeField] private Transform playerStatsListTransform;
    private Dictionary<long, PlayerStateItem> playerStatsItemDic = new Dictionary<long, PlayerStateItem>();

    [Header("倒计时")]
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private float timer;

    [Header("退出游戏")]
    [SerializeField] private GameObject exitGamePanel;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    private void OnDisable()
    {
        facade.EventCenter.OnHealthChanged -= OnHealthChanged;
    }

    protected override void Start()
    {
        exitGameRequest = GetComponent<LeaveGameRequest>();
        facade = GameFacade.Instance;
        facade.EventCenter.OnHealthChanged += OnHealthChanged;
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
        facade.SwitchActionMap("Player");
    }

    private void OnConfirmBtnClick()
    {
        exitGameRequest.SendRequest();
        exitGamePanel.SetActive(false);
    }

    /// <summary>
    /// 更新玩家列表
    /// </summary>
    /// <param name="playerList"></param>
    public void UpdateList(List<PlayerInfo> playerList)
    {
        for (int i = playerStatsListTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerStatsListTransform.GetChild(i).gameObject);
        }

        playerStatsItemDic.Clear();

        foreach (var player in playerList)
        {
            GameObject o = Instantiate(playerStatsItem, playerStatsListTransform, false);
            PlayerStateItem item = o.GetComponent<PlayerStateItem>();
            item.Init(player.PlayerName, player.Health);
            playerStatsItemDic.Add(player.PlayerId, item);
        }
    }

    /// <summary>
    /// 更新生命值
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="health"></param>
    public void OnHealthChanged(long playerId, int health)
    {
        if (playerStatsItemDic.TryGetValue(playerId, out PlayerStateItem item))
        {
            item.UpdateHealth(health);
        }
    }

    public void ShowLeaveGamePanel()
    {
        facade.SwitchActionMap("UI");
        exitGamePanel.SetActive(true);
    }

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
