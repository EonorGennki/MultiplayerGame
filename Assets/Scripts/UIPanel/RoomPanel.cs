using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    [SerializeField] private Button leaveBtn;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button readyBtn;
    [SerializeField] private Button sendBtn;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI playerNum;
    [SerializeField] private TextMeshProUGUI chatTextField;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Transform playerListTransform;
    [SerializeField] private Transform msgListTransform;
    [SerializeField] private GameObject playerItemPrefab;
    [SerializeField] private GameObject msgItemPrefab;

    private LeaveRoomRequest leaveRoomRequest;
    private ChatRequest chatRequest;
    private StartGameRequest startGameRequest;
    private ReadyRequest readyRequest;
    public List<PlayerInfo> playerList { get; set; } = new List<PlayerInfo>();

    private bool isHost = false;
    private bool isProcessingReady = false; //防抖处理

    protected override void Start()
    {
        leaveRoomRequest = GetComponent<LeaveRoomRequest>();
        chatRequest = GetComponent<ChatRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
        readyRequest = GetComponent<ReadyRequest>();

        base.Start();
    }

    private void OnLeaveBtnClick()
    {
        leaveRoomRequest.SendRequest();
    }

    private void OnStartBtnClick()
    {
        startGameRequest.SendRequest();
    }

    private void OnReadyClick()
    {
        if (isProcessingReady)
        {
            return;
        }
        isProcessingReady = true;

        readyRequest.SendRequest();
    }

    private void OnSendBtnClick()
    {
        string text = Regex.Replace(chatTextField.text, "[\u200B-\u200D\uFEFF]", ""); ;

        if (text == string.Empty)
        {
            uiManager.ShowTooltip(PanelType.RoomTooltip, "发送内容不可为空！");
        }

        chatRequest.SendRequest(text);
    }

    /// <summary>
    /// 非本地更新玩家列表
    /// </summary>
    public void UpdatePlayerList()
    {
        for (int i = playerListTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerListTransform.GetChild(i).gameObject);
        }

        foreach (var player in playerList)
        {
            GameObject gameObject = Instantiate(playerItemPrefab, playerListTransform, false);
            PlayerItem item = gameObject.GetComponent<PlayerItem>();
            item.SetPlayerInfo(player);
        }
    }

    /// <summary>
    /// 本地更新玩家列表
    /// </summary>
    /// <param name="playerList"></param>
    private void UpdatePlayerList(List<PlayerInfo> playerList)
    {
        if (isHost)
        {
            readyBtn.gameObject.SetActive(false);
        }
        else
        {
            startBtn.gameObject.SetActive(false);
        }

        for (int i = playerListTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerListTransform.GetChild(i).gameObject);
        }

        foreach (var player in playerList)
        {
            GameObject gameObject = Instantiate(playerItemPrefab, playerListTransform, false);
            PlayerItem item = gameObject.GetComponent<PlayerItem>();
            item.SetPlayerInfo(player);

            if (player.UserId == GameManager.Instance.LocalUserId)
            {
                SetButtonText(player);
            }
        }
    }

    /// <summary>
    /// 刷新消息列表
    /// </summary>
    /// <param name="text"></param>
    public void UpdateChatList(string text)
    {
        GameObject gameObject = Instantiate(msgItemPrefab, msgListTransform, false);
        TextMeshProUGUI msg = gameObject.GetComponent<TextMeshProUGUI>();
        msg.text = text;
        chatTextField.text = string.Empty;

        if (msgListTransform.childCount > 20)
        {
            Destroy(msgListTransform.GetChild(0));
        }
    }

    /// <summary>
    /// 刷新房间信息
    /// </summary>
    /// <param name="roomInfo"></param>
    public void UpdateRoomInfo(RoomInfo roomInfo)
    {
        string str1 = "房间名：";
        roomName.text = str1 + roomInfo.roomName;
        string str2 = "房间人数：";
        playerNum.text = str2 + roomInfo.currentNum + "/" + roomInfo.maxNum;
    }

    /// <summary>
    /// 更新某位玩家状态
    /// </summary>
    /// <param name="playerName"></param>
    public void UpdatePlayersState(PlayerInfo player)
    {
        for (int i = 0; i < playerListTransform.childCount; i++)
        {
            PlayerItem item = playerListTransform.GetChild(i).GetComponent<PlayerItem>();
            PlayerInfo playerInfo = item.GetPlayerInfo();
            if (playerInfo.UserId == GameManager.Instance.LocalUserId)
            {
                SetButtonText(player);
            }

            if (playerInfo.PlayerName == player.PlayerName)
            {
                item.SetPlayerInfo(player);
                break;
            }
        }

        isProcessingReady = false;
    }

    public void SetIsHost(bool isHost) => this.isHost = isHost;

    public void SetButtonText(PlayerInfo player)
    {
        if (player.IsReady)
        {
            readyBtn.GetComponentInChildren<TextMeshProUGUI>().text = "取消准备";
            return;
        }
        readyBtn.GetComponentInChildren<TextMeshProUGUI>().text = "准备";
    }

    public void AutoLeaveRoom()
    {
        for (int i = msgListTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(msgListTransform.GetChild(i).gameObject);
        }
        uiManager.PopPanel();
    }

    private void AddListeners()
    {
        leaveBtn.onClick.AddListener(OnLeaveBtnClick);
        startBtn.onClick.AddListener(OnStartBtnClick);
        sendBtn.onClick.AddListener(OnSendBtnClick);
        readyBtn.onClick.AddListener(OnReadyClick);
        uiManager.OnPlayerListUpdate += UpdatePlayerList;
        uiManager.OnRoomInfoUpdate += UpdateRoomInfo;
        uiManager.OnIsHostSet += SetIsHost;
    }

    private void RemoveListeners()
    {
        leaveBtn.onClick.RemoveAllListeners();
        startBtn.onClick.RemoveAllListeners();
        sendBtn.onClick.RemoveAllListeners();
        readyBtn.onClick.RemoveAllListeners();
        uiManager.OnPlayerListUpdate -= UpdatePlayerList;
        uiManager.OnRoomInfoUpdate -= UpdateRoomInfo;
        uiManager.OnIsHostSet -= SetIsHost;
    }

    private void Show()
    {
        gameObject.SetActive(true);
        AddListeners();
    }

    private void Hide()
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
