using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    private Button joinBtn;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI playerNumber;
    [SerializeField] private TextMeshProUGUI roomState;

    public RoomListPanel roomListPanel;

    private void Start()
    {
        joinBtn = GetComponent<Button>();
        joinBtn.onClick.AddListener(OnJoinClick);
    }

    private void OnJoinClick()
    {
        roomListPanel.JoinRoom(roomName.text);
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.roomName;
        playerNumber.text = roomInfo.currentNum + "/" + roomInfo.maxNum;
        roomState.text = roomInfo.state;
    }
}
