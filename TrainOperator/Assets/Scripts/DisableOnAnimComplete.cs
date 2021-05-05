using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAnimComplete : MonoBehaviour
{
   public void DisableThisGameObject()
    {
        gameObject.SetActive(false);
    }

   
}
