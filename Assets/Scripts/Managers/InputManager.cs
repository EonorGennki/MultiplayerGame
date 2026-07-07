using UnityEngine.InputSystem;

public class InputManager : BaseManager
{
    private PlayerInputSet playerInput;
    private InputActionMap curretMap;

    private string currentMapName;

    public InputManager() : base()
    {
        playerInput = new PlayerInputSet();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnInit()
    {
        base.OnInit();

        playerInput = new PlayerInputSet();

        SwitchMap("UI");
    }

    public void SwitchMap(string mapName)
    {
        if (currentMapName == mapName)
        {
            return;
        }

        if (curretMap is not null)
        {
            curretMap.Disable();
        }

        switch (mapName)
        {
            case "Player":
                curretMap = playerInput.Player;
                break;
            case "UI":
                curretMap = playerInput.UI;
                break;
        }
    }
}
