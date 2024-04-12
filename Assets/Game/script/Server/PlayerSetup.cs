using UnityEngine;
using Mirror;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public Animator animator;


    [Header("Baby")]
    public GameObject baby_UI;
    public CapsuleCollider baby_Collider;
    public GameObject baby_Renderer;
    public GameObject weapon;
    public TextMeshProUGUI babyboo_timer;
    public TextMeshProUGUI babyboo_paperInGame;

    public RuntimeAnimatorController _animation;

    [Header("Paper")]
    public GameObject paper_UI;
    public CapsuleCollider paper_Collider;
    public GameObject paper_Renderer;
    public TextMeshProUGUI paper_TextMeshProUGUI;
    public TextMeshProUGUI paper_timer;

    [Header("W/L")]
    public GameObject winUI;
    public GameObject loseUI;

    [SyncVar(hook = nameof(HandleIsBabyboo))]
    public bool isBabyboo;

    private void HandleIsBabyboo(bool oldValue, bool newValue)
    {
        paper_UI.SetActive(!isBabyboo) ;
        paper_Collider.enabled = !isBabyboo;
        paper_Renderer.SetActive(!isBabyboo);

        baby_UI.SetActive(isBabyboo);
        baby_Collider.enabled = isBabyboo;
        baby_Renderer.SetActive(isBabyboo);
        weapon.SetActive(isBabyboo);

        animator.runtimeAnimatorController = _animation;
    }


    public bool isAlreadySet = false;


    [SyncVar(hook = nameof(HandleIsInGame))]
    private bool IsInGame;

    private void HandleIsInGame(bool oldValue, bool newValue)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lobby_UI.gameObject.SetActive(!IsInGame);
    }

    private void Update()
    {
        GameManager.Instance.DisplayKeyCount(paper_TextMeshProUGUI);

        babyboo_timer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(GameManager.Instance.time / 60), GameManager.Instance.time % 60);

        paper_timer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(GameManager.Instance.time / 60), GameManager.Instance.time % 60);



        if (GameManager.Instance.time <= 0f && isBabyboo)
        {
            winUI.SetActive(!GameManager.Instance.isPapermenWon);
            loseUI.SetActive(GameManager.Instance.isPapermenWon);

            StartCoroutine(Delay(5));

            SceneManager.LoadScene(0);
        }
        else if (GameManager.Instance.time <= 0f && !isBabyboo)
        {
            winUI.SetActive(GameManager.Instance.isPapermenWon);
            loseUI.SetActive(!GameManager.Instance.isPapermenWon);

            StartCoroutine(Delay(5));

            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
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

            // d�sactive les composants si ce n'est pas � nous
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
