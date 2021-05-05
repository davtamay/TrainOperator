using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WaitForEngineer : MonoBehaviour
{
    public UnityEvent onEngineerFound;

   

    public NetworkManager overridedNM;

    public GameObject trainParentTransform;
    
    public void OnEnable()
    {

        if(!NetworkManager.singleton.isNetworkActive)
        overridedNM.StartHost();

      


        StartCoroutine(WaitForEngineerFunction());
    }

    NetworkCalls netCallRef;

    public IEnumerator WaitForEngineerFunction()
    {
        
        yield return new WaitUntil(() => {


            return NetworkServer.connections.Count == 2;
        
        
        });


        //      EngineerInputState.INSTANCE.SetConductorState(true);


        //yield return new WaitUntil(() =>
        //{
        //    return EngineerInputState.isEngineerReady;
        //});




        if (!netCallRef)
            netCallRef = netCallRef = NetworkManager.singleton.client.connection.playerControllers[0].gameObject.GetComponent<NetworkCalls>();
      
        netCallRef.REInstantiateTrain();

        gameObject.SetActive(false);

        onEngineerFound.Invoke();


        GameObject engineer = null;
        
        while(engineer == null)
        {

            yield return new WaitForSeconds(1);
            engineer = GameObject.FindGameObjectWithTag("Engineer");

            Debug.Log(engineer.name);
        }
           
        engineer.SetActive(false);
      
    }

    //void Update()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        Debug.Log("F");

    //        CmdInstantiateTrain();
    //    }
    //}

    //[Command]
    //void CmdInstantiateTrain()
    //{
    //    // Create the Bullet from the Bullet Prefab
    //    var train = (GameObject)Instantiate(
    //        trainParentTransform
    //        );

    //    Debug.Log(train.name);

    //    // Spawn the train on the Clients
    //    NetworkServer.Spawn(train);

    // //   var trainPosition = train.GetComponent<TrainPosition>();
     

    //}


}
