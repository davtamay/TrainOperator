using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable]
public class UnityEventVector3 : UnityEvent<Vector3> { }
public class TrainPosition : NetworkBehaviour
{
    [SerializeField]
    public UnityEvent onTrackTriggerEnter;

    [SerializeField]
    public UnityEvent onCountDownTrigger;

 //   public UnityEventVector3 sendTrainPosTo;
    public int trainSpeed = 1;
    public enum DIRECTION {LEFT, RIGHT, CENTER }

    public int zPos = 10;

    public Transform thisTransform;

    public EngineerInputState engineerInputState;

    public Transform EngineerTransform;
    public Transform ConductorTransform;

    bool isTrainMoving = true;

    SetupGridPiece setupGridPiece;

    Vector3 initialTrainPos;

    [Header("Conductor Properties")]
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    public GameObject hornIndicator;
    public GameObject brakeIndicator;

    public GameObject power1;
    public GameObject power2;
    public GameObject power3;
    public GameObject power4;
    public GameObject power5;

    public LocalizationManager lm;

    public AudioSource asForBell;
    public void Start()
    {
        thisTransform = transform;
        setupGridPiece = GetComponent<SetupGridPiece>();
        initialTrainPos = transform.position;


        engineerInputState = NetworkManager.singleton.client.connection.playerControllers[0].gameObject.GetComponent<EngineerInputState>();


        // GameOverCanvas = GameObject.FindGameObjectWithTag("GameOver");
        //  GameOverCanvas.SetActive(false);
        CanvasController.INSTANCE.languageCanvas.SetActive(false);

        lm.SetLocalizedTextUpdate();
    }
    

    public static int wayPoint;
    List<Vector3> wayPointPositions = new List<Vector3>();
    public void Update()
    {
        //if (!isTrainMoving)
        //{
        //    if (wayPointPositions.Count >= wayPoint)
        //    {
        //        if (Vector3.Distance(wayPointPositions[wayPoint], thisTransform.position) < 0.2f)
        //        {

        //        }
        //        else
        //        {



        //            var dir = (wayPointPositions[wayPoint] - thisTransform.position).normalized;

        //            thisTransform.Translate((dir * 1) * Time.deltaTime, Space.World);

        //            thisTransform.forward = Vector3.Lerp(thisTransform.forward, dir, 2 * Time.deltaTime);

        //        }
        //    }
        //    else
        //        return;

        //}

       

        if (wayPointPositions.Count != 0)
        {
            if (wayPointPositions.Count - 1 < wayPoint) //[wayPoint] = null)
                return;

            var dir = (wayPointPositions[wayPoint] - thisTransform.position).normalized;

            thisTransform.Translate((dir * 1) * Time.deltaTime, Space.World);

            thisTransform.forward = Vector3.Lerp(thisTransform.forward, dir, 1.8f * Time.deltaTime);


            if (Vector3.Distance(wayPointPositions[wayPoint], thisTransform.position) < 0.2f)
            {
                if (wayPointPositions.Count > wayPoint)
                    wayPoint += 1;

            }
        }
        else
        thisTransform.Translate(Vector3.forward * Time.deltaTime, Space.World);
    }

    public void OnTriggerEnter(Collider other)
    {
        //int currentWayPointPos;
        if (other.CompareTag("Positions"))
        {

            StartCoroutine(InvokeCountDownAfterSeconds(2));

            if (other.TryGetComponent(out PathInputAndOutput pIO))
            {
               // Debug.Log(wayPointPositions.Count);
                switch (pIO.pathTypeEnum)
                {
                    case PathType.DIRECTION:

                        //set second which is the default or center pos

                        wayPointPositions.Add(pIO.inputPaths[2].position);
                        //wayPointPositions.Add(pIO.inputPaths[EngineerInputState.serverDirectionInt].position);

                        //   wayPointPositions.Add(transform.position + (Vector3.forward * Mathf.Infinity));
                        // wayPointPositions.Add(pIO.OutputPaths[0].position);


                        break;

                    case PathType.INCLINE:

                        foreach (var item in pIO.inclinePaths)
                        {
                            wayPointPositions.Add(item.position);
                        }

                        //if we do not match our incline power level we do game over
                        //if (EngineerInputState.power != pIO.inclineLevel)
                        //    SetGameOver();
                     //  /inputPaths[(int)engineerInputState.trainDir].position);
                      //  wayPointPositions.Add(pIO.OutputPaths[0].position);

                        break;

                    case PathType.OBSTACLE:


                        foreach (var item in pIO.obstaclePaths)
                            wayPointPositions.Add(item.position);

                     break;

                    case PathType.STATION:

                        //if(!asForBell.isPlaying)
                        //asForBell.Play();

                        wayPointPositions.Add(pIO.stationPaths[0].position);
                        wayPointPositions.Add(pIO.stationPaths[2].position);
                        //foreach (var item in pIO.obstaclePaths)
                        //    wayPointPositions.Add(item.position);


                        stationGameOverTransform = pIO.stationPaths[2];
                        stationWonTransform = pIO.stationPaths[1];
                       

                        break;


                }



            }







            if (other.TryGetComponent(out IndicatorInfo Iinfo))
            {
                switch (Iinfo.pathType)
                {
                    case PathType.DIRECTION:



                        //left is right
                        if (Iinfo.pathCorrect == 0)
                        {
                            StartCoroutine(TurnOnIndicatorForTime(leftIndicator));
                        }
                        else
                        {
                            StartCoroutine(TurnOnIndicatorForTime(rightIndicator));
                        }
                        break;

                    case PathType.OBSTACLE:
                        StartCoroutine(CheckForHorn(Iinfo.cowAnimator));
                        StartCoroutine(TurnOnIndicatorForTime(hornIndicator));

                        break;

                    case PathType.INCLINE:

                        if (Iinfo.powerLevel == 1)
                            StartCoroutine(TurnOnIndicatorForTime(power1));
                        else if (Iinfo.powerLevel == 2)
                            StartCoroutine(TurnOnIndicatorForTime(power2));
                        else if (Iinfo.powerLevel == 3)
                            StartCoroutine(TurnOnIndicatorForTime(power3));
                        else if (Iinfo.powerLevel == 4)
                            StartCoroutine(TurnOnIndicatorForTime(power4));
                        else if (Iinfo.powerLevel == 5)
                            StartCoroutine(TurnOnIndicatorForTime(power5));

                        //StartCoroutine(CheckForHorn(pIO.cowAnimator));
                        //StartCoroutine(TurnOnIndicatorForTime(hornIndicator));

                        break;

                    case PathType.STATION:

                        StartCoroutine(CheckForBrake());
                        StartCoroutine(TurnOnIndicatorForTime(brakeIndicator));

                        break;
                }


            }













        }

        if (other.CompareTag("TrackEnd"))
        {
            setupGridPiece.CreateAndSetPiece(new Vector3(0, 0, zPos));
            zPos += 10;

            other.gameObject.SetActive(false);

            onTrackTriggerEnter.Invoke();

            if (other.TryGetComponent(out PathInputAndOutput pIO))
            {
             //   Debug.Log(wayPointPositions.Count);
                switch (pIO.pathTypeEnum)
                {
                    case PathType.DIRECTION:

                        wayPointPositions[wayPointPositions.Count - 1] = pIO.inputPaths[EngineerInputState.serverDirectionInt].position;
                        wayPointPositions.Add(pIO.OutputPaths[0].position);


                        break;

                    case PathType.INCLINE:

                        //foreach (var item in pIO.inclinePaths)
                        //{
                        //    wayPointPositions.Add(item.position);
                        //}

                        //if we do not match our incline power level we do game over
                        if (EngineerInputState.power != pIO.inclineLevel)
                            SetGameOver();
                        //  /inputPaths[(int)engineerInputState.trainDir].position);
                        //  wayPointPositions.Add(pIO.OutputPaths[0].position);

                        break;

                    case PathType.OBSTACLE:


                        //foreach (var item in pIO.obstaclePaths)
                        //    wayPointPositions.Add(item.position);

                        break;

                    case PathType.STATION:


                        //foreach (var item in pIO.obstaclePaths)
                        //    wayPointPositions.Add(item.position);
                        //stationGameOverTransform = pIO.stationPaths[0];


                      //  StartCoroutine(CheckForBrake());

                        //IF OUR BRAKE IS NOT ON WHEN WE MAKE IT THEN WE ALLOW FOR GAME OVER?
                        //if (!EngineerInputState.wasBrakePressed)
                        //{
                        //    if (wayPointPositions[wayPointPositions.Count - 1] != null)
                        //    wayPointPositions[wayPointPositions.Count - 1] = pIO.stationPaths[0].position;

                        //    //won?
                        //}
                        //else
                        //   StartCoroutine(SetGameWon());

                        break;


                }



            }



        }



        if (other.CompareTag("Obstacle"))
        {
            SetGameOver();
            // NetworkServer.Destroy(transform.gameObject);
        }
    }

    public IEnumerator InvokeCountDownAfterSeconds(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        onCountDownTrigger.Invoke();
    }

    public void SetGameOver()
    {
        CanvasController.INSTANCE.GameOverUI.SetActive(true);
        CanvasController.INSTANCE.UICamera.SetActive(true);

        Destroy(transform.gameObject, 2);

       // isTrainMoving = true;
        //    EngineerInputState.serverDirectionInt = 0;
        wayPointPositions.Clear();
        zPos = 10;

    }

    public IEnumerator SetGameWon()
    {
        //to observe celebration?
        yield return new WaitForSeconds(3);

        //Debug.Log("TRIGGERED");
        CanvasController.INSTANCE.GameWonUI.SetActive(true);
        CanvasController.INSTANCE.UICamera.SetActive(true);

        Destroy(transform.gameObject, 2);

      //  isTrainMoving = true;
        //    EngineerInputState.serverDirectionInt = 0;
        wayPointPositions.Clear();
        zPos = 10;

    }

    public IEnumerator TurnOnIndicatorForTime(GameObject indicator)
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(5);
        indicator.SetActive(false);
    }

    public IEnumerator CheckForHorn(Animator animator)
    {
        float time = 0;
        // indicator.SetActive(true);
        yield return new WaitUntil(() => {

           // Debug.Log(time );

            time += Time.deltaTime;
          

            if (EngineerInputState.wasHornPressed)
            {
                animator.SetTrigger("LEAVE");
                return true;
            }

            //it takes seven seconds to reach the cow
            if (time > 8)
                return true;
            else
                return false;

        });

    }

    public Transform stationGameOverTransform;
    public Transform stationWonTransform;
    public IEnumerator CheckForBrake()
    {
        float time = 0;

        wayPointPositions[wayPointPositions.Count - 1] = stationGameOverTransform.position;

        //wait first
        yield return new WaitForSeconds(3.5f);

        yield return new WaitUntil(() => {

            Debug.Log(time);

            time += Time.deltaTime;


            if (EngineerInputState.wasBrakePressed)
            {

                wayPointPositions[wayPointPositions.Count - 1] = stationWonTransform.position;
                StartCoroutine(  SetGameWon());

                return true;
            }

            //two seconds to press brake
            if (time > 3f)
            {
                Debug.Log("Click");
                wayPointPositions[wayPointPositions.Count - 1] =stationGameOverTransform.position;
                return true;
               
            }
            else
                return false;

        });

    }


    public void OnDestroy()
    {
        //  EngineerInputState.serverDirectionInt = 0;
        wayPoint = 0;
        wayPointPositions.Clear();
        zPos = 10;

        if (isServer) 
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();


        CanvasController.INSTANCE.languageCanvas.SetActive(true);
    }







}
