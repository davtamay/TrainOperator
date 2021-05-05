using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class resetAfterToggle : MonoBehaviour
{
   // public float seconds;

    private Toggle thisToggle;
    public EngineerInputState eIS;

    public bool isHorn;
    public bool isBrake;

    public GameObject localPlayer;

    public AudioSource asource;
    public void Start()
    {
        localPlayer = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;
        eIS = localPlayer.GetComponent<EngineerInputState>();

        thisToggle = GetComponent<Toggle>();
    }
   
    public void InitiateResetToggle(bool flag)
    {
     

        if (isTimerActive)
        {
            thisToggle.isOn = true;

            Debug.Log("TIMER ACTIVE");
            return;
        }
          




        StartCoroutine(ResetToggle());


        if (isHorn)
        {
            if (EngineerInputState.INSTANCE == null)
            {

                localPlayer = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;

                EngineerInputState.INSTANCE = localPlayer.GetComponent<EngineerInputState>();


            }

            if (!asource.isPlaying)
                asource.Play();

            EngineerInputState.INSTANCE.CmdSendHornStatusToServer(1);
            EngineerInputState.wasHornPressed = true;
        }

        if (isBrake) {
           
            if (EngineerInputState.INSTANCE == null)
            {

                localPlayer = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;

                EngineerInputState.INSTANCE = localPlayer.GetComponent<EngineerInputState>();
            }

            EngineerInputState.INSTANCE.CmdSendBrakeStatusToServer(1);
            EngineerInputState.wasBrakePressed = true;
        }

    }

    bool isTimerActive = false;
    public IEnumerator ResetToggle()
    {
        isTimerActive = true;
        yield return new WaitForSeconds(1);


        isTimerActive = false;


        thisToggle.isOn = false;

        if (isHorn)
        {
            EngineerInputState.INSTANCE.CmdSendHornStatusToServer(0);
             EngineerInputState.wasHornPressed = false;
        }
        if (isBrake)
        {
            EngineerInputState.INSTANCE.CmdSendBrakeStatusToServer(0);
            EngineerInputState.wasBrakePressed = false;
        }
    }
}
