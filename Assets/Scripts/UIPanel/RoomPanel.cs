using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    [SerializeField] private Button leaveBtn;
    [SerializeField] private Button startBtn;
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
    public List<PlayerInfo> playerList = new List<PlayerInfo>();

    protected override void Start()
    {
        leaveRoomRequest = GetComponent<LeaveRoomRequest>();
        chatRequest = GetComponent<ChatRequest>();

        base.Start();
    }

    private void OnLeaveBtnClick()
    {
        leaveRoomRequest.SendRequest();
    }

    private void OnStartBtnClick()
    {

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

    public void ShowPlayerList()
    {
        UpdatePlayerList();
    }

    /// <summary>
    /// 更新玩家列表
    /// </summary>
    private void UpdatePlayerList()
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

    private void UpdatePlayerList(List<PlayerInfo> playerList)
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

    public void UpdateRoomInfo(RoomInfo roomInfo)
    {
        string str1 = "房间名：";
        roomName.text = str1 + roomInfo.roomName;
        string str2 = "房间人数：";
        playerNum.text = str2 + roomInfo.currentNum + "/" + roomInfo.maxNum;
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
        uiManager.OnPlayerListUpdate += UpdatePlayerList;
        uiManager.OnRoomInfoUpdate += UpdateRoomInfo;
    }

    private void RemoveListeners()
    {
        leaveBtn.onClick.RemoveAllListeners();
        startBtn.onClick.RemoveAllListeners();
        sendBtn.onClick.RemoveAllListeners();
        uiManager.OnPlayerListUpdate -= UpdatePlayerList;
        uiManager.OnRoomInfoUpdate -= UpdateRoomInfo;
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
