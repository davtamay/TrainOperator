using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum TRAINDIRECTION {RIGHT = 0, LEFT = 1 };

public class ValueMessage : MessageBase
{
    public NetworkInstanceId netid;
    public int value;
}
public class EngineerInputState : NetworkBehaviour
{
    public static EngineerInputState INSTANCE { get { return instance; } set { instance = value; } }
    private static EngineerInputState instance;

    public void Awake()
    {
        if(isLocalPlayer)
            instance = this;


        NetworkManager.singleton.client.RegisterHandler(directionMessageID, DirectionMessage);
        NetworkManager.singleton.client.RegisterHandler(powerMessageID, PowerMessage);

        NetworkManager.singleton.client.RegisterHandler(hornMessageID, HornMessage);
        NetworkManager.singleton.client.RegisterHandler(brakeMessageID, BrakeMessage);

        NetworkManager.singleton.client.RegisterHandler(conductoressageID, ConductorReadyMessage);
        NetworkManager.singleton.client.RegisterHandler(EngineerMessageID, EngineerReadyyMessage);


    }


    public void Start()
    {
        if (isServer)
        {
            //default input
            SetRight(true);
            SetPower(1);
        }
    }

    //public void Update()
    //{
    //    Debug.Log(wasHornPressed);
    //}

    public static int serverDirectionInt;

    //public override void OnStartClient()
    //{
       

    //}

    //directional
    short directionMessageID = 1000;

    [Command]
    public void CmdSendDirectionStatusToServer(int newtrainDir)
    {
        var msg = new ValueMessage();
        msg.value = newtrainDir;
        msg.netid = netId;

        NetworkServer.SendToAll(directionMessageID, msg);

    }

    static void DirectionMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ValueMessage>();
        var player = ClientScene.FindLocalObject(msg.netid);
        EngineerInputState.serverDirectionInt = msg.value;

    }


    public static int power;

    //power
    short powerMessageID = 1001;


    [Command]
    public void CmdSendPowerStatusToServer(int powerVal)
    {
        var msg = new ValueMessage();
        msg.value = powerVal;
        msg.netid = netId;

        NetworkServer.SendToAll(powerMessageID, msg);

    }

    static void PowerMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ValueMessage>();
        var player = ClientScene.FindLocalObject(msg.netid);
        EngineerInputState.power = msg.value;

    }




 
    public static bool wasHornPressed;

    short hornMessageID = 1002;


    [Command]
    public void CmdSendHornStatusToServer(int hornVal)
    {
       
        var msg = new ValueMessage();
        msg.value = hornVal;
        msg.netid = netId;

        NetworkServer.SendToAll(hornMessageID, msg);

    }

    static void HornMessage(NetworkMessage netMsg)
    {
        Debug.Log("ReceiveHorn");
        var msg = netMsg.ReadMessage<ValueMessage>();
        var player = ClientScene.FindLocalObject(msg.netid);


        EngineerInputState.wasHornPressed =  msg.value == 1?true:false;
    }








    public static bool wasBrakePressed;

    short brakeMessageID = 1003;


    [Command]
    public void CmdSendBrakeStatusToServer(int brakeVal)
    {
        var msg = new ValueMessage();
        msg.value = brakeVal;
        msg.netid = netId;

        NetworkServer.SendToAll(brakeMessageID, msg);

    }

    static void BrakeMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ValueMessage>();
        var player = ClientScene.FindLocalObject(msg.netid);

        EngineerInputState.wasBrakePressed = msg.value == 1 ? true : false;
    }



    public void SetConductorState(bool active)
    {
     //   CmdSendConductorReadyStatusToServer(active ? 1 : 0);
    }

    public static bool isConductorReady;

    short conductoressageID = 1010;


    [Command]
    public void CmdSendConductorReadyStatusToServer(int conductorVal)
    {
        var msg = new ValueMessage();
        msg.value = conductorVal;
        msg.netid = netId;

        NetworkServer.SendToAll(conductoressageID, msg);

    }

    static void ConductorReadyMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ValueMessage>();
        var player = ClientScene.FindLocalObject(msg.netid);

        EngineerInputState.isConductorReady = msg.value == 1 ? true : false;
    }

    public void SetEngineerState(bool active)
    {
       
        CmdSendEngineerReadyStatusToServer(active ? 1 : 0);
    }

    public static bool isEngineerReady;

    short EngineerMessageID = 1005;


    [Command]
    public void CmdSendEngineerReadyStatusToServer(int EngineerReadyVal)
    {
        var msg = new ValueMessage();
        msg.value = EngineerReadyVal;
        msg.netid = netId;

        Debug.Log("Engineer ready");
        NetworkServer.SendToAll(EngineerMessageID, msg);

    }

    static void EngineerReadyyMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<ValueMessage>();
        var player = ClientScene.FindLocalObject(msg.netid);

        EngineerInputState.isEngineerReady = msg.value == 1 ? true : false;
    }


    private void OnDestroy()
    {
        instance = null;
    }
    public void SetLeft(bool na)
    {
        CmdSendDirectionStatusToServer(1); 
    
    
    }
    public void SetRight(bool na)
    {
        CmdSendDirectionStatusToServer(0); 
    }


    public void SetPower(int power)
    {
        CmdSendPowerStatusToServer(power);
      //  this.power = power;

    }
   

    public void HornPressed()
    {
        CmdSendHornStatusToServer(1);
        //  wasHornPressed = true;

    }
    //public IEnumerator resetHorn()
    //{
    //    yield return new WaitForSeconds(2);
    //    CmdSendHornStatusToServer(0);
    //}
}
