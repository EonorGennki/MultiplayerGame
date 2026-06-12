using System.Collections.Generic;
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
    [SerializeField] private TextMeshProUGUI messageInputBox;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Transform playerListTransform;
    [SerializeField] private GameObject playerItemPrefab;

    private LeaveRoomRequest leaveRoomRequest;
    public List<PlayerInfo> playerList = new List<PlayerInfo>();

    protected override void Start()
    {
        leaveRoomRequest = GetComponent<LeaveRoomRequest>();

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

    public void UpdateRoomInfo(RoomInfo roomInfo)
    {
        string str1 = "房间名：";
        roomName.text = str1 + roomInfo.roomName;
        string str2 = "房间人数：";
        playerNum.text = str2 + roomInfo.currentNum + "/" + roomInfo.maxNum;
    }

    public void AutoLeaveRoom()
    {
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
