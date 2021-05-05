using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkCalls : NetworkBehaviour
{
    public GameObject trainPrefab;

    public void REInstantiateTrain()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        StartCoroutine(InstantiateTrain());
            //CmdInstantiateTrain();
        
    }
    
    public IEnumerator InstantiateTrain()
    {
        //if(isServer)
        //    EngineerInputState.INSTANCE.SetConductorState(true);
        //if(isLocalPlayer)
        //    EngineerInputState.INSTANCE.SetEngineerState(true);

        yield return new WaitUntil(()=>    NetworkServer.connections.Count == 2);
        CmdInstantiateTrain();
    }

    [Command]
    public void CmdInstantiateTrain()
    {
        // Create the Bullet from the Bullet Prefab
        var train = (GameObject)Instantiate(
            trainPrefab
            );

           NetworkServer.Spawn(train);
  
    }

  
}
