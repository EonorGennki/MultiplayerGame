using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    private CreateRoomRequest createRoomRequest;
    private SearchRoomRequest searchRoomRequest;
    [SerializeField] private Button backbtn;
    [SerializeField] private Button searchBtn;
    [SerializeField] private Button createbtn;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private Slider maxNum;
    [SerializeField] private Transform roomListTransform;
    [SerializeField] private GameObject roomItemPrefab;

    public List<RoomInfo> roomInfoList = new List<RoomInfo>();

    protected override void Start()
    {
        createRoomRequest = GetComponent<CreateRoomRequest>();
        searchRoomRequest = GetComponent<SearchRoomRequest>();
    }

    private void OnBackBtnClick() => uiManager.PopPanel();

    private void OnSearchBtnClick()
    {
        searchRoomRequest.SendRequest();
    }

    private void OnCreateBtnClick()
    {
        string roomName = Regex.Replace(this.roomName.text, "[\u200B-\u200D\uFEFF]", "");
        if (roomName == string.Empty)
        {
            uiManager.ShowTooltip(PanelType.RoomTooltip, "房间名不能为空！");
            return;
        }
        createRoomRequest.SendRequest(roomName, (int)maxNum.value);
    }

    public void ShowRoomTooltip<T>(bool success, string str) where T : BaseRequest
    {
        uiManager.ShowTooltip(PanelType.RoomTooltip, str);
        if (typeof(T) == typeof(CreateRoomRequest))
        {
            if (success == true)
            {
                uiManager.PushPanel(PanelType.Room);
            }
        }
        else if (typeof(T) == typeof(SearchRoomRequest))
        {
            if (success == true)
            {
                UpdateRoomList();
            }
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

    /// <summary>
    /// 更新房间列表
    /// </summary>
    public void UpdateRoomList()
    {
        //更新前清空房间列表
        for (int i = 0; i < roomListTransform.childCount; i++)
        {
            Destroy(roomListTransform.GetChild(i).gameObject);
        }

        foreach (var roomInfo in roomInfoList)
        {
            GameObject gameObject = Instantiate(roomItemPrefab, roomListTransform, false);
            RoomItem item = gameObject.GetComponent<RoomItem>();
            item.SetRoomInfo(roomInfo.roomName, roomInfo.currentNum, roomInfo.maxNum, roomInfo.state);
        }
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
