using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class SetServerOrClientStatus : NetworkBehaviour
{
    NetworkManager networkManager;

    Transform trainParentTransform;
    Transform engineerTransform;
    Transform conductorTransform;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    public void StartHost()
    {
      //  networkManager.StartHost();
    }

    public void StartClient()
    {
   //   var networkClient =  networkManager.StartClient();
       
    }

 

    public IEnumerator WaitForPartyToJoinTheGame()
    {

        yield return null;
    }
}
