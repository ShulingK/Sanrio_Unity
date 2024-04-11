using UnityEngine;
using Mirror;
using TMPro;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] components_to_disable;

    [SyncVar(hook = nameof(HandleSpawnPos))]
    public Vector3 spawnPos;
    private void HandleSpawnPos(Vector3 oldValue, Vector3 newValue)
    {
        transform.position = spawnPos;
    }

    private NetworkManagerLobby room;

    public NetworkRoomPlayerLobby lobby_UI;

    [Header("Baby")]
    public GameObject baby_UI;
    public CapsuleCollider baby_Collider;
    public GameObject baby_Renderer;
    public GameObject weapon;
    public TextMeshProUGUI babyboo_timer;
    public TextMeshProUGUI babyboo_bullet;
    public TextMeshProUGUI babyboo_reload;
    public TextMeshProUGUI babyboo_paperInGame;



    [Header("Paper")]
    public GameObject paper_UI;
    public CapsuleCollider paper_Collider;
    public GameObject paper_Renderer;
    public TextMeshProUGUI paper_TextMeshProUGUI;
    public TextMeshProUGUI paper_timer;

    [SyncVar(hook = nameof(HandleIsBabyboo))]
    public bool isBabyboo;
    
    private void HandleIsBabyboo(bool oldValue, bool newValue)
    {
        paper_UI.SetActive(false) ;
        paper_Collider.enabled = !isBabyboo;
        paper_Renderer.SetActive(!isBabyboo);

        baby_UI.SetActive(isBabyboo);
        baby_Collider.enabled = isBabyboo;
        baby_Renderer.SetActive(isBabyboo);
        weapon.SetActive(isBabyboo);
    }


    public bool isAlreadySet = false;


    private PlayerShoot playerShoot;

    [SyncVar(hook = nameof(HandleIsInGame))]
    private bool IsInGame;

    private void HandleIsInGame(bool oldValue, bool newValue)
    {
        lobby_UI.gameObject.SetActive(!IsInGame);
    }

    private void Start()
    {
        playerShoot = gameObject.GetComponent<PlayerShoot>();
    }

    private void Update()
    {
        GameManager.Instance.DisplayKeyCount(paper_TextMeshProUGUI);    

        if (isBabyboo)
        {
            babyboo_timer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(GameManager.Instance.time / 60), GameManager.Instance.time % 60);
            babyboo_bullet.text = playerShoot.currentWeapon.bullet.ToString();
            babyboo_reload.text = playerShoot.currentWeapon.bulletMax.ToString();
        }
        else
        {
            paper_timer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(GameManager.Instance.time / 60), GameManager.Instance.time % 60);
        }
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