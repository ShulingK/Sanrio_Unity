using TMPro;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Time")]
    [SerializeField]
    [SyncVar]
    public float time = 0f;

    [SerializeField]
    private float startTime;

    [SerializeField]
    private float gameDuration = 480f;

    [Header("GameData")]
    [SerializeField]
    [SyncVar]
    private int keyCount = 0;
    public int keyCountMax = 4;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        time = gameDuration; 
        startTime = Time.time;
    }

    void Update()
    {
        time = gameDuration - (int)Time.time - startTime;   
    }

    public void StartGame()
    {
        gameDuration = 480f;
    }

    public void AddKey()
    {
        keyCount++;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    public void DisplayKeyCount(TextMeshProUGUI textKeyCount)
    {
        textKeyCount.text = keyCount.ToString();
    }
}
