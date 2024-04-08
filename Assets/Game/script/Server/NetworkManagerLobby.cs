using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene][SerializeField] private string menuScene = string.Empty;

    [SerializeField]
    public GameObject object_to_update;


    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayersPrefab = null;


    public static event Action onClientConnected;
    public static event Action onClientDisconnected;

    public List<PlayerSetup> roomPlayers { get; } = new List<PlayerSetup>();
    public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();
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
        if(SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("OnServerAddPlayer");
        /*
        foreach (var c in NetworkServer.connections.Values)
        {
            if (c != conn)
            {
                roomPlayerUI.UpdateDisplay();
                return;
            }
        }
        */

        if (SceneManager.GetActiveScene().path == menuScene )
        {
            Debug.Log("Instantiate");
            bool isLeader = roomPlayers.Count == 0;

            
            GameObject player = Instantiate(playerPrefab);
            

            player.GetComponent<PlayerSetup>().lobby_UI.IsLeader = isLeader;


            NetworkServer.AddPlayerForConnection(conn, player);
        }
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

        ServerChangeScene("Scene_Map_01");
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name != newSceneName && newSceneName.StartsWith("Scene_Map"))
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
