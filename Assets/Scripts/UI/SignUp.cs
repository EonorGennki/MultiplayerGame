using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    private SignUpRequest SignUpRequest;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI password;
    [SerializeField] private TextMeshProUGUI Tip1;
    [SerializeField] private TextMeshProUGUI Tip2;
    [SerializeField] private Button SignUpBtn;

    private void Start()
    {
        SignUpRequest = GetComponent<SignUpRequest>();

        SignUpBtn.onClick.AddListener(OnSignUpBtnClick);
    }

    private void OnSignUpBtnClick()
    {
        //去除零宽字符
        string _username = Regex.Replace(username.text, "[\u200B-\u200D\uFEFF]", "");
        string _password = Regex.Replace(password.text, "[\u200B-\u200D\uFEFF]", "");

        if (_username == string.Empty)
        {
            Tip1.text = "用户名不能为空！";
            Tip1.color = Color.red;
            return;
        }
        else if (_password == string.Empty)
        {
            Tip1.text = "密码不能为空！";
            Tip1.color = Color.red;
            return;
        }
        else
        {
            SignUpRequest.SendRequest(_username, _password);
        }
    }
}
