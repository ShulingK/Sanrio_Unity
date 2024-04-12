using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 1; 

    [Header("Sections")]
    [SerializeField] private List<GameObject> game_GameObject;

    [Header("Pool")]
    public Pool keyPool;
    public Pool boostPool;

    [Header("SpawnPoints")]
    [SerializeField] private List<GameObject> babybooSpawnPoints;
    [SerializeField] private List<GameObject> papermanSpawnPoints;
    [SerializeField] private List<GameObject> boostSpawnPoints;
    [SerializeField] private List<GameObject> keySpawnPoints;

    [Header("Object to Spawn")]
    [SerializeField] private GameObject boostPrefab;
    [SerializeField] private List<GameObject> keysPrefab;



    public static event Action onClientConnected;
    public static event Action onClientDisconnected;

    public List<PlayerSetup> roomPlayers { get; } = new List<PlayerSetup>();
    public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();


    public NetworkGamePlayerLobby roomPlayer;

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

        GameManager.Instance.StartGame();
    }

    public void UpdateGame()
    {
        foreach (GameObject go in game_GameObject)
            go.SetActive(true);


        foreach (var player in roomPlayers)
        {
            player.OnGameStart();
        }
        


        int Babyboos;
        if (roomPlayers.Count < 4)
            Babyboos = 1;
        else if (roomPlayers.Count >= 4 && roomPlayers.Count <= 6)
            Babyboos = 2;
        else
            Babyboos = 3;


        List<int> index = new List<int>();
        for (int i = 0; i < roomPlayers.Count; i++)
        {
            index.Add(i);
        }


        List<int> indexOfBabyToGenerate = new List<int>();
        for (int i = 0; i < Babyboos; i++)
        {
            int a = index[UnityEngine.Random.Range(0, index.Count)];

            index.Remove(a);

            indexOfBabyToGenerate.Add(a);
        }

        for (int i = 0; i < roomPlayers.Count; i++)
        {
            for (int j = 0; j < indexOfBabyToGenerate.Count; j++)
            {
                if (i == j)
                    roomPlayers[i].isBabyboo = true;
            }
        }

        //KEY 
        for (int i = 0; i < 4; i++)
        {
            GameObject go = keySpawnPoints[UnityEngine.Random.Range(0, keySpawnPoints.Count - i)];

            keyPool.spawn = go.transform.position;

            keySpawnPoints.Remove(go);
        }


        //BOOST
        for (int i = 0; i < 4; i++)
        {
            GameObject go = boostSpawnPoints[UnityEngine.Random.Range(0, boostSpawnPoints.Count)];

            boostPool.spawn = go.transform.position;

            boostSpawnPoints.Remove(go);
        }

        //BABYBOO
        for (int i = 0; i < roomPlayers.Count; i++)
        {
            if (roomPlayers[i].isBabyboo)
            {
                GameObject go = babybooSpawnPoints[UnityEngine.Random.Range(0, babybooSpawnPoints.Count)];

                roomPlayers[i].spawnPos = go.transform.position;
                
                babybooSpawnPoints.Remove(go);
            }
            else
            {
                GameObject go = papermanSpawnPoints[UnityEngine.Random.Range(0, papermanSpawnPoints.Count)];

                roomPlayers[i].spawnPos = go.transform.position;

                papermanSpawnPoints.Remove(go);
            }
        }
    }
}
