using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    //panel资源字典
    private Dictionary<PanelType, BasePanel> panelDic = new Dictionary<PanelType, BasePanel>();
    //panel路径字典
    private Dictionary<PanelType, string> panelPathDic = new Dictionary<PanelType, string>();

    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    private Transform canvasTransform;

    public UIManager() : base()
    {
    }

    public override void OnInit()
    {
        base.OnInit();

        InitPanel();
        canvasTransform = GameObject.Find("Canvas").transform;
    }

    /// <summary>
    /// 显示UI界面
    /// </summary>
    /// <param name="panelType"></param>
    private void PushPanel(PanelType panelType)
    {
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }
        BasePanel panel = SpawnPanel(panelType);
        panelStack.Push(panel);
        panel.OnEnter();
    }

    private void PopPanel()
    {
        if (panelStack.Count <= 0)
        {
            return;
        }

        BasePanel topPanel = panelStack.Pop();
    }

    /// <summary> 
    /// 生成UI界面
    /// </summary>
    /// <param name="panelType"></param>
    private BasePanel SpawnPanel(PanelType panelType)
    {
        if (panelPathDic.TryGetValue(panelType, out string path))
        {
            GameObject o = Resources.Load<GameObject>(path);
            GameObject.Instantiate(o, canvasTransform, false);
            BasePanel panel = o.GetComponent<BasePanel>();
            panelDic.Add(panelType, panel);
            return panel;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 初始化UI界面
    /// </summary>
    private void InitPanel()
    {
        panelDic.Clear();
        panelPathDic.Clear();

        string panelPath = "Prefabs/UIPanel/";
        string[] pathArray = new string[]
        { "TooltipPane" , "MainPanel", "LoginPanel", "SignPanel"};

        for (int i = 0; i < pathArray.Length; i++)
        {
            panelPathDic.Add((PanelType)i, panelPath + pathArray[0]);
        }
    }
}
