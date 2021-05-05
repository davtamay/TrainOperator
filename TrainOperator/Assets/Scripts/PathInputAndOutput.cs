using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathType {DIRECTION, INCLINE, OBSTACLE, STATION }
public class PathInputAndOutput : MonoBehaviour
{
    public PathType pathTypeEnum;
    [Header("Directional Parameters")]
    public List<Transform> inputPaths;
    public List<Transform> OutputPaths;

    [Header("Incline Parameters")]
    public int inclineLevel;
    public List<Transform> inclinePaths;

    [Header("Obstacle Parameters")]
    public List<Transform> obstaclePaths;
    public Animator obstacleAnimator;


    [Header("Obstacle Parameters")]
    public List<Transform> stationPaths;
}
