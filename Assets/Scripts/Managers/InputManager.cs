using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputManager : BaseManager
{
    public PlayerInputSet PlayerInput {  get; private set; }
    private InputActionMap currentMap;
    private string currentMapName;

    public InputManager() : base()
    {
        PlayerInput = new PlayerInputSet();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnInit()
    {
        base.OnInit();

        PlayerInput = new PlayerInputSet();

        SwitchMap("UI");
    }

    public void SwitchMap(string mapName)
    {
        if (currentMapName == mapName)
        {
            return;
        }

        if (currentMap is not null)
        {
            currentMap.Disable();
        }

        switch (mapName)
        {
            case "Player":
                currentMap = PlayerInput.Player;
                break;
            case "UI":
                currentMap = PlayerInput.UI;
                break;
        }

        currentMap.Enable();
        currentMapName = mapName;
    }
}
