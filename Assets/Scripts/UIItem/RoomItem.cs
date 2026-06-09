using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    private Button joinBtn;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI playerNumber;
    [SerializeField] private TextMeshProUGUI roomState;

    private void Start()
    {
        joinBtn = GetComponent<Button>();
        joinBtn.onClick.AddListener(OnJoinClick);
    }

    private void OnJoinClick()
    {

    }

    public void SetRoomInfo(string roomName, int currentNum, int maxNum, string roomState)
    {
        this.roomName.text = roomName;
        this.playerNumber.text = currentNum + "/" + maxNum;
        this.roomState.text = roomState;
    }
}
