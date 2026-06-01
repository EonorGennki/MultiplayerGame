using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : BasePanel
{
    private SignUpRequest SignUpRequest;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI password;
    [SerializeField] private TextMeshProUGUI Tip1;
    [SerializeField] private TextMeshProUGUI Tip2;
    [SerializeField] private Button SignUpBtn;
    [SerializeField] private Button BackBtn;

    private void Start()
    {
        SignUpRequest = GetComponent<SignUpRequest>();
    }

    private void OnSignUpBtnClick()
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
            SignUpRequest.SendRequest(username, password);
        }
    }

    private void OnBackBtnClick()
    {
        uiManager.PopPanel();
    }

    private void AddListeners()
    {
        SignUpBtn.onClick.AddListener(OnSignUpBtnClick);
        BackBtn.onClick.AddListener(OnBackBtnClick);
    }

    private void RemoveListeners()
    {
        SignUpBtn.onClick.RemoveAllListeners();
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
