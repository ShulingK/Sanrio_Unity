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
    Behaviour[] Objects_to_disable;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;




    public static event Action onClientConnected;
    public static event Action onClientDisconnected;

    public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

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

        if (isOwnCanva) { isOwnCanva = !isOwnCanva; }
        else
        {
            // désactive les composants si ce n'est pas à nous
            if (Objects_to_disable != null)
            {
                for (int i = 0; i < Objects_to_disable.Length; i++)
                {
                    Objects_to_disable[i].enabled = false;
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
}
