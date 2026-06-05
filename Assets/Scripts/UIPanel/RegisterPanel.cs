using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private RegisterRequest registerRequest;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI password;
    [SerializeField] private TextMeshProUGUI Tip1;
    [SerializeField] private TextMeshProUGUI Tip2;
    [SerializeField] private Button RegisterBtn;
    [SerializeField] private Button BackBtn;

    protected override void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();
    }

    private void OnBackBtnClick() => uiManager.PopPanel();

    private void OnRegisterBtnClick()
    {
        //去除零宽字符
        string username = Regex.Replace(this.username.text, "[\u200B-\u200D\uFEFF]", "");
        string password = Regex.Replace(this.password.text, "[\u200B-\u200D\uFEFF]", "");

        if (username == string.Empty)
        {
            Tip1.text = "*用户名不能为空！";
            Tip1.color = Color.red;
            return;
        }
        else if (password == string.Empty)
        {
            Tip1.text = "*密码不能为空！";
            Tip1.color = Color.red;
            return;
        }
        else
        {
            registerRequest.SendRequest(username, password);
        }
    }

    public void ShowAuthTooltip(bool success, string str)
    {
        uiManager.ShowTooltip(PanelType.AuthTooltip, str);

        if (success == true)
        {
            uiManager.PushPanel(PanelType.Login);
        }
    }

    private void AddListeners()
    {
        RegisterBtn.onClick.AddListener(OnRegisterBtnClick);
        BackBtn.onClick.AddListener(OnBackBtnClick);
    }

    private void RemoveListeners()
    {
        RegisterBtn.onClick.RemoveAllListeners();
        BackBtn.onClick.RemoveAllListeners();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        AddListeners();
    }

    protected void Hide()
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
