using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InputState : MonoBehaviour
{
    public GameObject localPlayer;
    public EngineerInputState eIS;

    public bool isLeft;
    public bool isRight;

    public bool isPower;
    public int powerVal;

  
    

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;
        eIS = localPlayer.GetComponent<EngineerInputState>();


        if (isLeft)
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((var) => eIS.SetLeft(false)) ;
        }
        if (isRight)
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((var) => eIS.SetRight(false));
        }
        
        if (isPower)
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((var) => eIS.SetPower(powerVal));
        }
    }


   
}
