using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] components_to_disable;

    private NetworkManagerLobby room;

    public NetworkRoomPlayerLobby lobby_UI;

    [Header("Baby")]
    public GameObject baby_UI;
    public CapsuleCollider baby_Collider;
    public GameObject baby_Renderer;
    public GameObject weapon;


    [Header("Paper")]
    public GameObject paper_UI;
    public CapsuleCollider paper_Collider;
    public GameObject paper_Renderer;

    public bool isBabyboo = false;

    [SyncVar(hook = nameof(HandleIsInGame))]
    private bool IsInGame;

    private void HandleIsInGame(bool oldValue, bool newValue)
    {
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
        if (isBabyboo)
        {
            baby_UI.gameObject.SetActive(IsInGame);
            paper_Collider.enabled = !IsInGame;
            baby_Renderer.SetActive(IsInGame);
            weapon.SetActive(IsInGame);
        }
        else
        {
            paper_UI.gameObject.SetActive(IsInGame);
            baby_Collider.enabled = !IsInGame;
            paper_Renderer.SetActive(IsInGame);
        }
        transform.position = Vector3.zero;

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