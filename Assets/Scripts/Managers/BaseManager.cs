using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected GameFace face;

    public BaseManager(GameFace _face)
    {
        face = _face;
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnDestroy()
    {

    }
}
