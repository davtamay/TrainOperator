using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WaitForConductor : NetworkBehaviour
{
    public UnityEvent onCondunctorFound;

   
    public NetworkManager overridedNM;

    Transform trainParentTransform;
    //Transform engineerTransform;
    public void OnEnable()
    {
       if (!NetworkManager.singleton.isNetworkActive)
        overridedNM.StartClient();//StartClient();
        

        StartCoroutine(WaitForConductorFunction());
    }
    

    //public void OnPlayerConnected(Net player)
    //{
    //    pla
    //    EngineerInputState.INSTANCE.SetEngineerState(true);
    //}

    //public override void OnStartLocalPlayer()
    //{
    //    EngineerInputState.INSTANCE.SetEngineerState(true);
    //}
    public IEnumerator WaitForConductorFunction()
    {
        
        yield return new WaitUntil(() => {


            return overridedNM.IsClientConnected(); //&& EngineerInputState.INSTANCE;


        }
        );


       

       // yield return new WaitUntil(() =>
       // {
       //     return EngineerInputState.isConductorReady;
       // }
       //);


        gameObject.SetActive(false);
        onCondunctorFound.Invoke();


        GameObject conductor = null;

        while (conductor == null)
        {

            yield return new WaitForSeconds(1);
            conductor = GameObject.FindGameObjectWithTag("Conductor");

            Debug.Log(conductor.name);
        }

        conductor.SetActive(false);
        //var conductor = GameObject.FindGameObjectWithTag("Conductor");
        //conductor.SetActive(false);
        //var trainPosition = train.GetComponent<TrainPosition>();

        //overridedNM.playerSpawnPos = trainPosition.EngineerTransform;


    }





}
