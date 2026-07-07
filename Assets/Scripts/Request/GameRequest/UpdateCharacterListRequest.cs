using SocketGameProtocal;
using System.Collections.Generic;

public class UpdateCharacterListRequest : BaseRequest
{
    private InGamePanel inGamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateCharacterList;

        base.Awake();
    }

    public override void Start()
    {
        inGamePanel = GetComponent<InGamePanel>();

        base.Start();
    }

    public override void OnResponse(MainPack pack)
    {
        List<PlayerInfo> playerList = ToPlayerList(pack);

        PlayerInfo removedPlayer = playerList[0];
        playerList.RemoveAt(0);

        mainContext.Post(_ =>
        {
            inGamePanel.UpdateList(playerList);
            facade.RemovePlayer(removedPlayer.PlayerId);
        }
        , null);
    }

    private List<PlayerInfo> ToPlayerList(MainPack pack)
    {
        List<PlayerInfo> playerList = new List<PlayerInfo>();
        foreach (var player in pack.PlayerPack)
        {
            PlayerInfo playerInfo = ToPlayerInfo(player);
            playerList.Add(playerInfo);
        }
        return playerList;
    }

    private PlayerInfo ToPlayerInfo(PlayerPack playerPack)
    {
        long playerId = playerPack.PlayerId;
        string playerName = playerPack.PlayerName;
        bool isReady = playerPack.IsReady;
        int health = playerPack.Health;
        PlayerInfo playerInfo = new PlayerInfo(playerId, playerName, isReady, health);
        return playerInfo;
    }
}
