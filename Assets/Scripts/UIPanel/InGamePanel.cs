using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    private GameFacade Facade => GameFacade.Instance;

    private LeaveGameRequest exitGameRequest;
    private GameOverRequest gameOverRequest;

    [Header("玩家列表")]
    [SerializeField] private GameObject playerStatsItem;
    [SerializeField] private Transform playerStatsListTransform;
    private Dictionary<long, PlayerStateItem> playerStatsItemDic = new Dictionary<long, PlayerStateItem>();

    [Header("倒计时")]
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private float time;
    private float timer;
    private bool canCountDown;

    [Header("退出游戏")]
    [SerializeField] private GameObject exitGamePanel;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button confirmBtn;

    [Header("游戏结束")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button exitBtn;

    protected override void Start()
    {
        exitGameRequest = GetComponent<LeaveGameRequest>();
        gameOverRequest = GetComponent<GameOverRequest>();
    }

    private void FixedUpdate()
    {
        if (!canCountDown)
        {
            return;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer< 0)
            {
                timer = 0;
                canCountDown = false;
                gameOverRequest.SendRequest(Facade.LocalPlayerId);
            }

            countDown.text = Mathf.Floor(timer).ToString();
        }
    }

    private void OnCancelBtnClick()
    {
        exitGamePanel.SetActive(false);
        Facade.SwitchActionMap("Player");
    }

    private void OnConfirmBtnClick()
    {
        exitGameRequest.SendRequest();
        exitGamePanel.SetActive(false);
    }

    private void OnExitBtnClick()
    {
        exitGameRequest.SendRequest();
        gameOverPanel.SetActive(false);
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

    public void OnScoreChanged(long playerId, int score)
    {
        if (playerStatsItemDic.TryGetValue(playerId, out PlayerStateItem item))
        {
            item.UpdateScore(score);
        }
    }

    public void ShowLeaveGamePanel()
    {
        Facade.SwitchActionMap("UI");
        exitGamePanel.SetActive(true);
    }

    public void ShowGameOverPanel(string text)
    {
        Facade.SwitchActionMap("UI");
        gameOverPanel.SetActive(true);
        gameOverText.text = text;
    }

    private void AddListeners()
    {
        exitBtn.onClick.AddListener(OnExitBtnClick);
        cancelBtn.onClick.AddListener(OnCancelBtnClick);
        confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        uiManager.OnPlayerListUpdate += UpdateList;
        Facade.EventCenter.OnHealthChanged += OnHealthChanged;
        Facade.EventCenter.OnScoreChanged += OnScoreChanged;
    }

    private void RemoveListeners()
    {
        exitBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
        uiManager.OnPlayerListUpdate -= UpdateList;
        Facade.EventCenter.OnHealthChanged -= OnHealthChanged;
        Facade.EventCenter.OnHealthChanged -= OnScoreChanged;
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
        timer = time;
        canCountDown = true;
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
