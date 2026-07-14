using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManeger : BaseManager
{
    private Dictionary<long, GameObject> players = new Dictionary<long, GameObject>();

    private long localPlayerId;

    private PlayerController playerController;
    private GameObject character;
    private GameObject[] spawnPoint;

    private BulletPool bulletPool => BulletPool.Instance;

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

            if (player.PlayerId == localPlayerId)
            {
                o.AddComponent<UpdateCharacterStateRequest>();
                o.AddComponent<StateSync>();
                o.AddComponent<GunController>();
                o.AddComponent<PlayerController>();
            }
            else
            {
                o.AddComponent<GunController>();
                o.AddComponent<SyncController>();
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

    public void UpdateCharacterState(long playerId, StatePack statePack)
    {
        if (!players.TryGetValue(playerId, out GameObject playerObject))
        {
            return;
        }

        SyncController syncController = playerObject.GetComponent<SyncController>();
        syncController.Sync(statePack);
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
