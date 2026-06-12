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

    public List<PlayerInfo> playerInfoList = new List<PlayerInfo>();

    private void OnLeaveBtnClick() => uiManager.PopPanel();

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
        for (int i = 0; i < playerListTransform.childCount; i++)
        {
            Destroy(playerListTransform.GetChild(i).gameObject);
        }

        foreach (var playerInfo in playerInfoList)
        {
            GameObject gameObject = Instantiate(playerItemPrefab, playerListTransform, false);
            PlayerItem item = gameObject.GetComponent<PlayerItem>();
            item.SetPlayerInfo(playerInfo);
        }
    }

    private void UpdatePlayerList(List<PlayerInfo> playerList)
    {
        for (int i = 0; i < playerListTransform.childCount; i++)
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
        roomName.text = roomName.text + roomInfo.roomName;
        playerNum.text = playerNum.text + roomInfo.currentNum + "/" + roomInfo.maxNum;
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
