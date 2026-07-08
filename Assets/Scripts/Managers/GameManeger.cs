using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManeger : BaseManager
{
    private Dictionary<long, GameObject> players = new Dictionary<long, GameObject>();

    private long localPlayerId;

    private GameObject character;
    private GameObject[] spawnPoint;

    public override void OnInit()
    {
        base.OnInit();
        character = Resources.Load("Prefabs/Player") as GameObject;
    }

    public void AddPlayer(List<PlayerInfo> playerList)
    {
        spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint").OrderBy(go => go.name).ToArray();

        int i = 0;
        foreach (var player in playerList)
        {
            GameObject o = GameObject.Instantiate(character, spawnPoint[i].transform.position, Quaternion.identity);
            i++;

            PlayerController playerController = o.GetComponent<PlayerController>();
            if (player.PlayerId == localPlayerId)
            {
                playerController.Init(true);
            }
            else
            {
                playerController.Init(false);
            }

            players.Add(player.PlayerId, o);
        }
    }

    public void RemovePlayer(long playerId)
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
    
    public void UpdateCharacterState(long playerId, Input input, Vector2 aimTargetPos)
    {
        if (!players.TryGetValue(playerId, out GameObject player))
        {
            return;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.SetInput(input);
        playerController.SetAimTarget(aimTargetPos);
    }

    public void Clear()
    {
        foreach (var player in players.Values)
        {
            GameObject.Destroy(player);
        }

        players.Clear();
    }
}
