using System;
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

    public event Action<List<PlayerInfo>> OnPlayerListUpdate;
    public event Action<RoomInfo> OnRoomInfoUpdate;
    public event Action<bool> OnIsHostSet;

    public UIManager() : base()
    {
    }

    public override void OnInit()
    {
        base.OnInit();

        InitPanel();
        canvasTransform = GameObject.Find("Canvas").transform;
        PushPanel(PanelType.Main);
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
        { "AuthTooltipPanel", "RoomTooltipPanel", "MainPanel", "LoginPanel", "RegisterPanel",
          "RoomListPanel", "RoomPanel", "InGamePanel"};

        for (int i = 0; i < pathArray.Length; i++)
        {
            panelPathDic.Add((PanelType)i, panelPath + pathArray[i]);
        }
    }

    /// <summary>
    /// 显示UI界面
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(PanelType panelType)
    {
        if (!panelDic.TryGetValue(panelType, out BasePanel panel))
        {
            panel = SpawnPanel(panelType);
        }

        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        panelStack.Push(panel);
        panel.OnEnter();
        return panel;
    }

    /// <summary>
    /// 关闭当前UI界面
    /// </summary>
    public void PopPanel()
    {
        if (panelStack.Count <= 0)
        {
            return;
        }

        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count > 0)
        {
            BasePanel panel = panelStack.Peek();
            panel.OnResume();
        }
    }

    /// <summary> 
    /// 生成UI界面
    /// </summary>
    /// <param name="panelType"></param>
    private BasePanel SpawnPanel(PanelType panelType)
    {
        if (panelPathDic.TryGetValue(panelType, out string path))
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            GameObject o = GameObject.Instantiate(prefab, canvasTransform, false);
            BasePanel panel = o.GetComponent<BasePanel>();
            panel.UIManager = this;
            panelDic.Add(panelType, panel);
            return panel;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 显示tooltip
    /// </summary>
    /// <param name="panelType"></param>
    /// <param name="str"></param>
    public void ShowTooltip(PanelType panelType, string str)
    {
        if (!panelDic.TryGetValue(panelType, out BasePanel panel))
        {
            panel = SpawnPanel(panelType);
        }

        if (panel is ITooltipPanel tooltipPanel)
        {
            tooltipPanel.Show(str);
        }
    }

    /// <summary>
    /// 显示离开游戏面板
    /// </summary>
    public void ShowLeaveGamePanel()
    {
        if (panelDic.TryGetValue(PanelType.InGame, out BasePanel panel))
        {
            InGamePanel inGamePanel = panel as InGamePanel;
            inGamePanel.ShowLeaveGamePanel();
        }
    }

    public void TriggerIsHostSet(bool isHost) => OnIsHostSet?.Invoke(isHost);
    public void TriggerRoomInfoUpdate(RoomInfo roomInfo) => OnRoomInfoUpdate?.Invoke(roomInfo);
    public void TriggerPlayerListUpdate(List<PlayerInfo> playerList) => OnPlayerListUpdate?.Invoke(playerList);
}
