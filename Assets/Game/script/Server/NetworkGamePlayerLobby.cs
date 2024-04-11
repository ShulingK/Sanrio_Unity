using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string DisplayName = "Loading...";

    [SyncVar]
    private List<int> spawnOfBabies = new List<int>();
    [SyncVar]
    private List<GameObject> ListOfBoosts = new List<GameObject>();
    [SyncVar]
    private List<GameObject> ListOfKeys = new List<GameObject>();


    [Header("SpawnPoints")]
    [SerializeField] private List<GameObject> babybooSpawnPoints;
    [SerializeField] private List<GameObject> papermanSpawnPoints;
    [SerializeField] private List<GameObject> boostSpawnPoints;
    [SerializeField] private List<GameObject> keySpawnPoints;

    [Header("Object to Spawn")]
    [SerializeField] private GameObject boostPrefab;
    [SerializeField] private List<GameObject> keysPrefab;

    public NetworkManagerLobby networkManager;


    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.gamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.gamePlayers.Remove(this);
    }
    [Server]
    public void SetDisplayName(string displayName)
    {
        this.DisplayName = displayName;
    }


    public void GenerateMap()
    {
        int Babyboos;
        if (networkManager.roomPlayers.Count < 4)
            Babyboos = 1;
        else if (networkManager.roomPlayers.Count >= 4 && networkManager.roomPlayers.Count <= 6)
            Babyboos = 2;
        else
            Babyboos = 3;


        List<int> index = new List<int>();
        for (int i = 0; i < networkManager.roomPlayers.Count; i++)
        {
            index.Add(i);
        }

        for (int i = 0; i < Babyboos; i++)
        {
            int a = index[UnityEngine.Random.Range(0, index.Count)];

            index.Remove(a);

            spawnOfBabies.Add(a);
        }



        //KEY 
        for (int i = 0; i < 4; i++)
        {
            GameObject go = keySpawnPoints[UnityEngine.Random.Range(0, keySpawnPoints.Count - i)];

            ListOfKeys.Add(Instantiate(keysPrefab[i], go.transform.position, UnityEngine.Quaternion.identity));

            keySpawnPoints.Remove(go);
        }


        //BOOST
        for (int i = 0; i < 4; i++)
        {
            GameObject go = boostSpawnPoints[UnityEngine.Random.Range(0, boostSpawnPoints.Count - i)];

            

            ListOfBoosts.Add(Instantiate(boostPrefab, go.transform.position, UnityEngine.Quaternion.identity));

            boostSpawnPoints.Remove(go);
        }

        //BABYBOO
        for (int i = 0; i < networkManager.roomPlayers.Count; i++)
        {
            if (networkManager.roomPlayers[i].isBabyboo)
            {
                GameObject go = babybooSpawnPoints[UnityEngine.Random.Range(0, babybooSpawnPoints.Count - i)];

                networkManager.roomPlayers[i].transform.position = go.transform.position;

                babybooSpawnPoints.Remove(go);
            }
            else
            {
                GameObject go = papermanSpawnPoints[UnityEngine.Random.Range(0, papermanSpawnPoints.Count - i)];

                networkManager.roomPlayers[i].transform.position = go.transform.position;

                papermanSpawnPoints.Remove(go);
            }
        }


    }

}
