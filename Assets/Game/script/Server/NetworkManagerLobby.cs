using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 1;


    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayersPrefab = null;

    [Header("Sections")]
    [SerializeField] private List<GameObject> game_GameObject;


    public static event Action onClientConnected;
    public static event Action onClientDisconnected;

    public List<PlayerSetup> roomPlayers { get; } = new List<PlayerSetup>();
    public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();


    
    [Header("SpawnPoints")]
    [SerializeField] private List<GameObject> babybooSpawnPoints;
    [SerializeField] private List<GameObject> papermanSpawnPoints;
    [SerializeField] private List<GameObject> boostSpawnPoints;
    [SerializeField] private List<GameObject> keySpawnPoints;

    [Header("Object to Spawn")]
    [SerializeField] private GameObject boostPrefab;
    [SerializeField] private List<GameObject> keysPrefab;


    /*
    private List<string> addressAlreadyConnected = new List<string>();
    */
    public override void OnClientConnect()
    {
        base.OnClientConnect();

        onClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        onClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        bool isLeader = roomPlayers.Count == 0;

            
        GameObject player = Instantiate(playerPrefab);
            

        player.GetComponent<PlayerSetup>().lobby_UI.IsLeader = isLeader;


        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<PlayerSetup>();
            
            roomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    

    public override void OnStopServer()
    {
        roomPlayers.Clear();    
    }
    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in roomPlayers)
        {
            player.lobby_UI.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in roomPlayers)
        {
            if (!player.lobby_UI.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if(!IsReadyToStart()) { return; }

        UpdateGame();
    }

    public void UpdateGame()
    {
        foreach (GameObject go in game_GameObject)
            go.SetActive(true);

        int[] Babyboos = new int[(int)Math.Floor((double)roomPlayers.Count / 3)];
        for(int i = 0; i < Babyboos.Length; i++)
        {
            roomPlayers[i].isBabyboo = true;
        }
        
        foreach (var player in roomPlayers)
        {
            player.OnGameStart();
        }

        //KEY 
        for(int i = 0; i < 4; i++)
        {
            GameObject go = keySpawnPoints[UnityEngine.Random.Range(0, keySpawnPoints.Count - i)];

            Instantiate(keysPrefab[i], go.transform.position, UnityEngine.Quaternion.identity);

            keySpawnPoints.Remove(go);
        }
        

        //BOOST
        for(int i = 0; i < 4; i++)
        {
            GameObject go = boostSpawnPoints[UnityEngine.Random.Range(0, keySpawnPoints.Count - i)];

            Instantiate(boostPrefab, go.transform.position, UnityEngine.Quaternion.identity);

            keySpawnPoints.Remove(go);
        }

        //BABYBOO
        for (int i = 0; i < roomPlayers.Count; i++)
        {
            if (roomPlayers[i].isBabyboo)
            {
                GameObject go = babybooSpawnPoints[UnityEngine.Random.Range(0, babybooSpawnPoints.Count - i)];

                roomPlayers[i].transform.position = go.transform.position;

                babybooSpawnPoints.Remove(go);
            }
            else
            {
                GameObject go = papermanSpawnPoints[UnityEngine.Random.Range(0, papermanSpawnPoints.Count - i)];

                roomPlayers[i].transform.position = go.transform.position;

                papermanSpawnPoints.Remove(go);
            }
        }


    }
}
