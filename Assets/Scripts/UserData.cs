using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int LocalUserId { get; private set; }
    public string LocalUsername { get; private set; }

    public UserData(int userId, string username)
    {
        LocalUserId = userId;
        LocalUsername = username;
    }
}
