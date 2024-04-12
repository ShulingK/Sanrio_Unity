using TMPro;
using UnityEngine;
using Mirror;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Time")]
    [SerializeField]
    [SyncVar]
    public float time = 100000f;

    [SerializeField]
    private float startTime;

    [SerializeField]
    private float gameDuration = 480f;

    [Header("GameData")]
    [SerializeField]
    [SyncVar]
    private int keyCount = 0;
    public int keyCountMax = 4;

    [SyncVar(hook = nameof(HandleIsPapermenWin))]
    public bool isPapermenWon = false;
    private void HandleIsPapermenWin(bool oldValue,  bool newValue)
    {
        time = 0f;
    }


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    void Update()
    {
        time = gameDuration - (int)Time.time - startTime;   
    }


    public void StartGame()
    {
        time = gameDuration;
        startTime = Time.time;
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
