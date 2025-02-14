using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class DataDisplay : MonoBehaviour
{
    [SerializeField] private ContinuousDrawSpline continuousDrawSplinePlayer;

    [SerializeField] private SplineContainer splineContainerTherapist;
    [SerializeField] private TMP_Text trajectoryLength;

    [SerializeField] private SplineContainer splineContainerPlayer;
    [SerializeField] private TMP_Text userPathLength;

    [SerializeField] private TMP_Text timeTaken;
    [SerializeField] private TMP_Text matchedAreaRatio;


    public void OnPathFinished()
    {
        trajectoryLength.text = (SplineUtility.CalculateLength(splineContainerTherapist.Spline, Matrix4x4.identity) * 10).ToString("#.###");
        userPathLength.text = (SplineUtility.CalculateLength(splineContainerPlayer.Spline, Matrix4x4.identity) * 10).ToString("#.###");
        timeTaken.text = continuousDrawSplinePlayer.TimeTaken.ToString("#.##") + "s";
    }


}
