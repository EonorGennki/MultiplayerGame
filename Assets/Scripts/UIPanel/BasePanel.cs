using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePanel : MonoBehaviour
{
    protected UIManager uiManager;
    public UIManager UIManager
    {
        set
        {
            uiManager = value;
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Show()
    {

    }

    protected virtual void Hide()
    {

    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnPause()
    {

    }

    public virtual void OnResume()
    {

    }

    public virtual void OnExit()
    {

    }
}
