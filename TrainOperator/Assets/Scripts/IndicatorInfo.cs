using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndicatorInfo : MonoBehaviour
{
    public PathType pathType;
    [Header("left = 0, right = 1")]
    //left = 0, right = 1
    public int pathCorrect;

    [Header("CownObstacle")]
    public Animator cowAnimator;


    [Header("Power")]
    public int powerLevel;
}