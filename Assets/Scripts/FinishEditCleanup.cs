using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;


public enum SplineDrawState { DrawMode, EditMode, PlayMode, Reset, PlayComplete}
/// <summary>
/// Script for cleaning up edit mode, when user confirms path
/// </summary>
public class FinishEditCleanup : MonoBehaviour
{
    [Header("Therapist")]
    [SerializeField] private SplineInstantiate splineInstantiateTherapist;
    [SerializeField] private SplineExtrude splineExtrudeTherapist;
    [SerializeField] private ContinuousDrawSpline continousDrawTherapist;
    [Header("Player")]
    [SerializeField] private SplineInstantiate splineInstantiatePlayer;
    [SerializeField] private SplineExtrude splineExtrudePlayer;
    [SerializeField] private ContinuousDrawSpline continousDrawPlayer;
    [SerializeField] private EditSpline editSpline;

    public void SplineDrawCleanup(SplineDrawState currentState)
    {
        switch (currentState)
        {
            case SplineDrawState.DrawMode: // If current state is draw state, switch to edit mode
                SplineStateManager.CurrentSplineState = SplineDrawState.EditMode;
                continousDrawTherapist.SetAnchorPointGrabability(true);
                continousDrawTherapist.CopySplineToVisualizeRadius(); // Does not work yet
                break;

            case SplineDrawState.EditMode: // If current state is edit mode, switch to play
                SplineStateManager.CurrentSplineState = SplineDrawState.PlayMode;
                continousDrawTherapist.enabled = false;
                splineExtrudeTherapist.enabled = true;
                DeactivateAnchorPointMeshAndPathVisualTherapist();
                break;

            case SplineDrawState.Reset: // Start path draw over completely
                SplineStateManager.CurrentSplineState = SplineDrawState.DrawMode;
                continousDrawTherapist.SplineContainer.Spline.Clear();
                splineExtrudeTherapist.GetComponent<MeshRenderer>().enabled = false;
                DeactivateAnchorPointMeshAndPathVisualTherapist();
                splineExtrudePlayer.GetComponent<MeshRenderer>().enabled = false;
                continousDrawPlayer.SplineContainer.Spline.Clear();
                DeactivateAnchorPointMeshAndPathVisualPlayer();
                break;

            case SplineDrawState.PlayMode: // If current state is play mode, Default/no function (yet)
                // TODO: spawn end point collider
                // TODO: generate collider
                break;
        }
    }

    public void DestroyAnchorPoints()
    {
        foreach (GameObject anchorVisual in continousDrawTherapist.AnchorPoints)
            Destroy(anchorVisual);

        continousDrawTherapist.AnchorPoints.Clear();
    }

    public void DeactivateAnchorPointMeshAndPathVisualTherapist()
    {
        splineInstantiateTherapist.enabled = false;

        continousDrawTherapist.SetAnchorPointVisibility(false);
    }

    public void DeactivateAnchorPointMeshAndPathVisualPlayer()
    {
        splineInstantiatePlayer.enabled = false;

        continousDrawPlayer.SetAnchorPointVisibility(false);
    }

    // Setup to use On Click with enums :((
    public void SplineDrawCleanupDrawMode() => SplineDrawCleanup(SplineDrawState.DrawMode);
    public void SplineDrawCleanupEditMode() => SplineDrawCleanup(SplineDrawState.EditMode);
    public void SplineDrawCleanupPlayMode() => SplineDrawCleanup(SplineDrawState.PlayMode);
    public void SplineDrawCleanupReset() => SplineDrawCleanup(SplineDrawState.Reset);
    public void SplineDrawCleanupPlayComplete() => SplineDrawCleanup(SplineDrawState.PlayComplete);


}
