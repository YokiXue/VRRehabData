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
            continuousDrawSplinePlayer.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") // if other is with the last anchor point
        {
            IsPathFinished = true;
        }
    }

}
