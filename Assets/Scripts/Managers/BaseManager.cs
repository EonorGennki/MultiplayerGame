using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    protected GameFace face;

    public BaseManager()
    {
        face = GameFace.Instance;
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
