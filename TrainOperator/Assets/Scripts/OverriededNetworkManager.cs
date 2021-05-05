using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OverriededNetworkManager : NetworkManager
{
    Transform trainTransform;
    public Transform playerSpawnPos;
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos.position, playerSpawnPos.rotation);
   //     player.transform.SetParent(trainTransform, true);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
