using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例基类
/// </summary>
/// <typeparam name="T"></typeparam>
[DisallowMultipleComponent]
//不允许在同一个物体上挂载多个继承该类的脚本
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    private static bool appIsQuitting = false;
    
    public static T Instance
    {
        get
        {
            if (appIsQuitting)
            {
                return null;
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance is null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        appIsQuitting = true;
    }
}
