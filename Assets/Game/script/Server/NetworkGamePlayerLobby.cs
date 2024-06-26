using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string DisplayName = "Loading...";

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
}
