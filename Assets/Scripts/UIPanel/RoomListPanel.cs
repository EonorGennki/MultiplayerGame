using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    [SerializeField] private Button backbtn;
    [SerializeField] private Button searchBtn;
    [SerializeField] private Button createbtn;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private Slider maxNum;
    [SerializeField] private Transform roomListTransform;
    [SerializeField] private GameObject roomItemPrefab;

    private CreateRoomRequest createRoomRequest;
    private SearchRoomRequest searchRoomRequest;
    private JoinRoomRequest joinRoomRequest;
    public List<RoomInfo> roomList = new List<RoomInfo>();

    private bool isProcessingCreate = false;
    private bool isProcessingSearch = false;

    protected override void Start()
    {
        createRoomRequest = GetComponent<CreateRoomRequest>();
        searchRoomRequest = GetComponent<SearchRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
    }

    private void OnBackBtnClick()
    {
        uiManager.PopPanel();
    }

    private void OnSearchBtnClick()
    {
        if (isProcessingSearch)
        {
            return;
        }
        isProcessingSearch = true;

        searchRoomRequest.SendRequest();
    }

    private void OnCreateBtnClick()
    {
        if (isProcessingCreate)
        {
            return;
        }
        isProcessingCreate = true;

        string roomName = Regex.Replace(this.roomName.text, "[\u200B-\u200D\uFEFF]", "");
        if (roomName == string.Empty)
        {
            uiManager.ShowTooltip(PanelType.RoomTooltip, "房间名不能为空！");
            return;
        }
        createRoomRequest.SendRequest(roomName, (int)maxNum.value);
    }

    public void JoinRoom(string roomName)
    {
        joinRoomRequest.SendRequest(roomName);
    }

    public void ShowRoomTooltip<T>(bool success, string str, List<PlayerInfo> playerList = null, RoomInfo roomInfo = null) where T : BaseRequest
    {
        uiManager.ShowTooltip(PanelType.RoomTooltip, str);
        if (typeof(T) == typeof(CreateRoomRequest))
        {
            isProcessingCreate = false;
            if (success)
            {
                uiManager.PushPanel(PanelType.Room);
                uiManager.OnIsHostSet.Invoke(true);
                uiManager.OnRoomInfoUpdate.Invoke(roomInfo);
                uiManager.OnPlayerListUpdate.Invoke(playerList);
            }
        }
        else if (typeof(T) == typeof(SearchRoomRequest))
        {
            isProcessingSearch = false;
            UpdateRoomList();
        }
        else if (typeof(T) == typeof(JoinRoomRequest))
        {
            if (success)
            {
                uiManager.PushPanel(PanelType.Room);
                uiManager.OnIsHostSet.Invoke(false);
                uiManager.OnRoomInfoUpdate.Invoke(roomInfo);
                uiManager.OnPlayerListUpdate.Invoke(playerList);
            }
        }

    }

    /// <summary>
    /// 更新房间列表
    /// </summary>
    public void UpdateRoomList()
    {
        //更新前清空房间列表
        for (int i = roomListTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(roomListTransform.GetChild(i).gameObject);
        }

        foreach (var roomInfo in roomList)
        {
            GameObject gameObject = Instantiate(roomItemPrefab, roomListTransform, false);
            RoomItem item = gameObject.GetComponent<RoomItem>();
            item.roomListPanel = this;
            item.SetRoomInfo(roomInfo);
        }
    }

    private void AddListeners()
    {
        backbtn.onClick.AddListener(OnBackBtnClick);
        searchBtn.onClick.AddListener(OnSearchBtnClick);
        createbtn.onClick.AddListener(OnCreateBtnClick);
    }

    private void RemoveListeners()
    {
        backbtn.onClick.RemoveAllListeners();
        searchBtn.onClick.RemoveAllListeners();
        createbtn.onClick.RemoveAllListeners();
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

    public override void OnExit()
    {
        base.OnExit();

        Hide();
    }
}
