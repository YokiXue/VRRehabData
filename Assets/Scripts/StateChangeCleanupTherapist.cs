using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class StateChangeCleanupTherapist : MonoBehaviour
{
    [Header("Therapist")]
    [SerializeField] private SplineInstantiate splineInstantiateTherapist;
    [SerializeField] private SplineExtrude splineExtrudeTherapist;
    [SerializeField] private ContinuousDrawSpline continousDrawTherapist;

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
                // TODO: enter ability to save spline
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
                break;

        }
    }

    public void DeactivateAnchorPointMeshAndPathVisualTherapist()
    {
        splineInstantiateTherapist.enabled = false;

        continousDrawTherapist.SetAnchorPointVisibility(false);
    }

    // Setup to use On Click with enums :((
    public void StateChangeCleanupDrawMode() => SplineDrawCleanup(SplineDrawState.DrawMode);
    public void StateChangeCleanupEditMode() => SplineDrawCleanup(SplineDrawState.EditMode);
    public void StateChangeCleanupPlayMode() => SplineDrawCleanup(SplineDrawState.PlayMode);
    public void StateChangeCleanupReset() => SplineDrawCleanup(SplineDrawState.Reset);
    public void StateChangeCleanupPlayComplete() => SplineDrawCleanup(SplineDrawState.PlayComplete);
}
