using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerTracingCompletion : MonoBehaviour
{
    [Header("Therapist")]
    [SerializeField] private ContinuousDrawSpline continuousDrawSplineTherapist;
    [SerializeField] private SplineContainer splineContainerTherapist;
    [Header("Player")]
    [SerializeField] private ContinuousDrawSpline continuousDrawSplinePlayer;
    [SerializeField] private SplineExtrude splineExtrudePlayer;
    [SerializeField] private SplineContainer splineContainerPlayer;
    [SerializeField] private Material boxColor;
    [Header("Data display")]
    //[SerializeField] private DataDisplay dataDisplay;
    [SerializeField] private TMP_Text trajectoryLength;
    [SerializeField] private TMP_Text userPathLength;
    [SerializeField] private TMP_Text timeTaken;
    [SerializeField] private TMP_Text matchedAreaRatio;

    public bool IsPathFinished { get; set; } = false;

    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (IsPathFinished)
        {
            meshRenderer.material = boxColor;
            continuousDrawSplinePlayer.SetAnchorPointGrabability(false);
            continuousDrawSplinePlayer.SetAnchorPointVisibility(false);
            splineExtrudePlayer.enabled = true;
            OnPathFinished();
            continuousDrawSplinePlayer.enabled = false;
        }

    }

    public void OnPathFinished()
    {
        trajectoryLength.text = (SplineUtility.CalculateLength(splineContainerTherapist.Spline, Matrix4x4.identity) * 10).ToString("#.###");
        userPathLength.text = (SplineUtility.CalculateLength(splineContainerPlayer.Spline, Matrix4x4.identity) * 10).ToString("#.###");
        timeTaken.text = continuousDrawSplinePlayer.TimeTaken.ToString("#.##") + "s";

        //trajectoryLength.text = splineContainerTherapist.Spline.GetLength().ToString();
        //userPathLength.text = splineContainerPlayer.Spline.GetLength().ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") // if other is with the last anchor point
        {
            IsPathFinished = true;
        }
    }

}
