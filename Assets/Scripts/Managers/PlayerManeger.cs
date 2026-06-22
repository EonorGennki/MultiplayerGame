using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : BaseManager
{
    private Dictionary<long, GameObject> players = new Dictionary<long, GameObject>();

    private long localPlayerId;

    private GameObject character;
    private Transform spawnPos;

    public override void OnInit()
    {
        base.OnInit();
        character = Resources.Load("Prefab/Character") as GameObject;
    }

    public void AddPlayer(List<PlayerInfo> playerList)
    {
        spawnPos = GameObject.Find("SpawnPos").transform;
        foreach (var player in playerList)
        {
            GameObject o = GameObject.Instantiate(character, spawnPos.position, Quaternion.identity);

            if (player.PlayerId == localPlayerId)
            {
                players.Add(player.PlayerId, o);
            }
        }
    }

    public void RemovePlayer(int playerId)
    {
        if (players.TryGetValue(playerId, out GameObject o))
        {
            players.Remove(playerId);
            GameObject.Destroy(o);
        }
        else
        {
            Debug.Log("É¾³ý½ÇÉ«³ö´í");
        }
    }

    public void SetLocalPlayerId(long playerId) => localPlayerId = playerId;

    public long GetLocalPlayerId() => localPlayerId;
}
