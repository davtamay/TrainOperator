using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController INSTANCE { get { return instance; } }
    private static CanvasController instance;

    //conductor = 0, engineer = 1
    public int roleID;

    public GameObject GameOverUI;
    public GameObject GameWonUI;

    public GameObject ConductorStartUI;
    public GameObject EngineerStartUI;

    public GameObject UICamera;

    public GameObject languageCanvas;

    public void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {



        }
    }

    public void SetRoleID(int id)
    {
        roleID = id;

    }

    public void TurnOnRoleStartUI()
    {
        GameOverUI.SetActive(false);

        if (roleID == 0)
            ConductorStartUI.SetActive(true);
        else
            EngineerStartUI.SetActive(true);


    }
}
