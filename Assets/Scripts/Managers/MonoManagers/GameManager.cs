using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int LocalUserId {  get; private set; }
    public string LocalUsername { get; private set; }

    public void SetUserInfo(int userId, string username)
    {
        LocalUserId = userId;
        LocalUsername = username;
    }
}
