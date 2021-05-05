using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupGridPiece : NetworkBehaviour
{
    public List<GameObject> prefabPieces = new List<GameObject>();

    public GameObject lastPiece;

    public int lastWaveLevel;


    public float delayCreatingPiece = 3;
    // public int zPos = 20;
    Transform pieceParent;

    public void Start()
    {
        



        if (isServer)
        {
            //DestroyOldPieces();

            //turn on our conductor for our host wich is the first child
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

        }
     
    }

    private int pieceLevel = 0;
    public void CreateAndSetPiece(Vector3 piecePos)
    {
        if (!isServer)
            return;

        //if(prefabPieces.Count -1 >= pieceLevel)
        //CmdInstantiatePiece(prefabPieces[pieceLevel]);
        //else
        //    CmdInstantiatePiece(prefabPieces[prefabPieces.Count - 1]);

        //pieceLevel++;
       
        
        //randomization
        //if( TrainPosition.wayPoint >= lastWaveLevel)
        //     StartCoroutine(CreateAndSetPieceEnumer(piecePos, true));
        //else
         StartCoroutine(CreateAndSetPieceEnumer(piecePos));

    }


    public IEnumerator CreateAndSetPieceEnumer(Vector3 piecePos, bool isFinalPiece = false)
    {
        yield return null;
        // yield return new WaitForSeconds(delayCreatingPiece);

        if (prefabPieces.Count - 1 >= pieceLevel)
            CmdInstantiatePiece(prefabPieces[pieceLevel], piecePos.z);
        else
            CmdInstantiatePiece(prefabPieces[pieceLevel], piecePos.z);

        pieceLevel++;



        //random
        //var ranNum = Random.Range(0, prefabPieces.Count);

        //GameObject ins = default;

        //if (isFinalPiece == false)
        //    CmdInstantiatePiece(prefabPieces[ranNum], piecePos.z);//, piecePos, Quaternion.AngleAxis(90, Vector3.right), transform.parent);
        //else
        //    CmdInstantiatePiece(lastPiece, piecePos.z);

    }

    public List<GameObject> currentSessionPieceList = new List<GameObject>();
    
    [Command]
    void CmdInstantiatePiece(GameObject piece, float zPos)
    {
        //delete old pieces if there are more than three
        if (currentSessionPieceList.Count >= 3)
            NetworkServer.Destroy(currentSessionPieceList[0]);


        // Create the Bullet from the Bullet Prefab
        var curPiece = (GameObject)Instantiate(
            piece, new Vector3(0, 0, zPos), Quaternion.AngleAxis(90, Vector3.right) );

        //Debug.Log(curPiece.name);
        // Spawn the bullet on the Clients
        NetworkServer.Spawn(curPiece);

        currentSessionPieceList.Add(curPiece);
 
    }

    public void DestroyOldPieces()
    {
        foreach (var item in currentSessionPieceList)
        {
            NetworkServer.Destroy(item);
        }

    }
    public void OnDestroy()
    {
        pieceLevel = 0;
        DestroyOldPieces();
    }

}
