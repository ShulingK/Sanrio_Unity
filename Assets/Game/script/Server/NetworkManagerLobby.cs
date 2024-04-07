using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 3;
    [Scene][SerializeField] private string menuScene = string.Empty;

    [SerializeField]
    Behaviour[] objects_to_disable;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayersPrefab = null;


    public static event Action onClientConnected;
    public static event Action onClientDisconnected;

    public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    private bool isOwnCanva = true; 

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
        if(SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }

        Debug.Log(isOwnCanva);

        if (isOwnCanva) { isOwnCanva = !isOwnCanva; }
        else
        {
            // désactive les composants si ce n'est pas à nous
            if (objects_to_disable != null)
            {
                for (int i = 0; i < objects_to_disable.Length; i++)
                {
                    objects_to_disable[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = roomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            
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
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in roomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().name == menuScene)
        {
            if(!IsReadyToStart()) { return; }

            ServerChangeScene("Scene_Map_01");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Map"))
        {
            for (int i = roomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = roomPlayers[i].connectionToClient;
                var gamePlayersInstance = Instantiate(gamePlayersPrefab);
                gamePlayersInstance.SetDisplayName(roomPlayers[i].name);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayersInstance.gameObject);
            }

            base.ServerChangeScene(newSceneName);
        }
    }
}
