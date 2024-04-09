using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] components_to_disable;

    [SerializeField]
    Camera defaultCamera;


    private NetworkManagerLobby room;


    public NetworkRoomPlayerLobby lobby_UI;

    [SyncVar(hook = nameof(HandleIsInGame))]
    private bool IsInGame;

    private void HandleIsInGame(bool oldValue, bool newValue)
    {
        lobby_UI.gameObject.SetActive(!IsInGame);
    }



    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            lobby_UI.gameObject.SetActive(false);

            // désactive les composants si ce n'est pas à nous
            if (components_to_disable != null)
            {
                for (int i = 0; i < components_to_disable.Length; i++)
                {
                    components_to_disable[i].enabled = false;
                }
            }
        }
        else
        {
            if (defaultCamera != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        if (defaultCamera != null)
            Camera.main.gameObject.SetActive(true);
    }


    public override void OnStartClient()
    {
        Room.roomPlayers.Add(this);

        lobby_UI.UpdateDisplay();

        transform.position = new Vector3(0, 500, 0);
    }

    public override void OnStopClient()
    {
        Room.roomPlayers.Remove(this);

        lobby_UI.UpdateDisplay();
    }

    public void OnGameStart()
    {
        IsInGame = true;
    }
}