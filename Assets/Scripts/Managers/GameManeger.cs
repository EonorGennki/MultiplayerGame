using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManeger : BaseManager
{
    private Dictionary<long, GameObject> playerDic = new Dictionary<long, GameObject>();

    private long localPlayerId;

    private GameObject character;
    private GameObject[] spawnPoint;

    public override void OnInit()
    {
        base.OnInit();
        character = Resources.Load("Prefabs/Player") as GameObject;
    }

    /// <summary>
    /// 生成玩家
    /// </summary>
    /// <param name="playerList"></param>
    public void SpawnPlayer(List<PlayerInfo> playerList)
    {
        spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint").OrderBy(go => go.name).ToArray();

        int i = 0;
        foreach (var player in playerList)
        {
            GameObject o = GameObject.Instantiate(character, spawnPoint[i].transform.position, Quaternion.identity);
            i++;

            o.AddComponent<GunController>().SetPlayerId(player.PlayerId);

            if (player.PlayerId == localPlayerId)
            {
                o.AddComponent<GainScoreRequest>();
                o.AddComponent<UpdateCharacterStateRequest>();
                o.AddComponent<StateSync>();
                o.AddComponent<PlayerHealth>().SetMaxHealth(player.Health);
                o.AddComponent<UpdateHealthRequest>();
                o.AddComponent<PlayerController>();
            }
            else
            {
                o.AddComponent<SyncController>();
            }

            playerDic.Add(player.PlayerId, o);
        }
    }

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="playerId"></param>
    public void RemovePlayer(long playerId)
    {
        if (playerDic.TryGetValue(playerId, out GameObject o))
        {
            playerDic.Remove(playerId);
            GameObject.Destroy(o);
        }
        else
        {
            Debug.Log("删除角色出错");
        }
    }

    /// <summary>
    /// 存储本地玩家id
    /// </summary>
    /// <param name="playerId"></param>
    public void SetLocalPlayerId(long playerId) => localPlayerId = playerId;

    /// <summary>
    /// 获取本地玩家id
    /// </summary>
    /// <returns></returns>
    public long GetLocalPlayerId() => localPlayerId;

    /// <summary>
    /// 位置同步
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="statePack"></param>
    public void UpdateCharacterState(long playerId, StatePack statePack)
    {
        if (!playerDic.TryGetValue(playerId, out GameObject playerObject))
        {
            return;
        }

        SyncController syncController = playerObject.GetComponent<SyncController>();
        syncController.Sync(statePack);
    }

    /// <summary>
    /// 清理玩家和列表
    /// </summary>
    public void Clear()
    {
        foreach (var player in playerDic.Values)
        {
            GameObject.Destroy(player);
        }

        playerDic.Clear();
    }

    /// <summary>
    /// 重生
    /// </summary>
    public void Respawn()
    {
        if (playerDic.TryGetValue(localPlayerId, out GameObject player))
        {
            player.SetActive(false);
            int index = Random.Range(0, spawnPoint.Length);
            player.transform.position = spawnPoint[index].transform.position;
            player.GetComponent<PlayerHealth>().ResetHealth();
            player.SetActive(true);
        }
    }

    public void GainScore()
    {

    }
}
