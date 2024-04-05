using Mirror;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [Scene][SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    public static event Action onClientConnected;
    public static event Action onClientDisconnected;

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList<GameObject>();

    public override void OnStartClient()
    {
        OnValidate();   
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }
    
    public override void OnClientConnect()
    {
        base.OnClientConnect();

        onClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientConnect();

        onClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
        if(SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }
}
