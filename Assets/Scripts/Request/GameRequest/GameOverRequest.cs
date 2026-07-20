using SocketGameProtocal;

public class GameOverRequest : BaseRequest
{
    private InGamePanel inGamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameOver;

        base.Awake();
    }

    public override void Start()
    {
        inGamePanel = GetComponent<InGamePanel>();

        base.Start();
    }

    public void SendRequest(long playerId)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        PlayerPack playerPack = new PlayerPack();
        playerPack.PlayerId = playerId;

        pack.PlayerPack.Add(playerPack);

        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        bool isWinner = pack.PlayerPack[0].IsWinner;
        string text = "";

        if (isWinner)
        {
            text = "Äã Ó® ÁË£¡";
        }
        else
        {
            text = "Äã Êä ÁË";
        }

            mainContext.Post(_ => inGamePanel.ShowGameOverPanel(text), null);
    }
}
