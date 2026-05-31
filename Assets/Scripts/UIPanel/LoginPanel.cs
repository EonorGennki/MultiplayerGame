using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private LoginRequest loginRequest;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI password;
    [SerializeField] private TextMeshProUGUI Tip1;
    [SerializeField] private TextMeshProUGUI Tip2;
    [SerializeField] private Button SignUpBtn;
    [SerializeField] private Button LoginBtn;
    [SerializeField] private Button BackBtn;

    private void OnSignUpBtnClick()
    {
        uiManager.PushPanel(PanelType.SignUp);
    }

    private void OnLoginBtnClick()
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
            loginRequest.SendRequest(username, password);
        }
    }

    private void OnBackBtnClick()
    {
        uiManager.PopPanel();
    }

    private void AddListeners()
    {
        SignUpBtn.onClick.AddListener(OnSignUpBtnClick);
        LoginBtn.onClick.AddListener(OnLoginBtnClick);
        BackBtn.onClick.AddListener(OnBackBtnClick);
    }

    private void RemoveListeners()
    {
        SignUpBtn.onClick.RemoveAllListeners();
        LoginBtn.onClick.RemoveAllListeners();
        BackBtn.onClick.RemoveAllListeners();
    }

    private void Display()
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

        Display();
    }

    public override void OnPause()
    {
        base.OnPause();

        Hide();
    }

    public override void OnResume()
    {
        base.OnResume();

        Display();
    }

    public override void OnExit()
    {
        base.OnExit();

        Hide();
    }
}
