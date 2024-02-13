using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.XR.Interaction.Toolkit;

public class EditSpline : MonoBehaviour
{

    [Header("Spline specifics from level build")]
    [SerializeField] private ContinuousDrawSpline continuousDrawSplineTherapist;
    [SerializeField] private SplineContainer splineContainerTherapist;
    [SerializeField] private SplineExtrude splineExtrudeTherapist;
    [SerializeField] private SplineInstantiate splineInstantiateTherapist;

    [Header("Spline specifics from player")]
    [SerializeField] private SplineInstantiate splineInstantiatePlayer;
    [SerializeField] private ContinuousDrawSpline continuousDrawSplinePlayer;

    private MeshRenderer splineMeshRenderer;

    private void Start()
    {
        splineMeshRenderer = splineContainerTherapist.GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode)
        {
            for (int i = 0; i < continuousDrawSplineTherapist.AnchorPoints.Count; i++)
            {
                if (continuousDrawSplineTherapist.AnchorPoints[i].GetComponent<XRGrabInteractable>().isSelected)
                {
                    BezierKnot knotToMove = continuousDrawSplineTherapist.SplineContainer.Spline.Knots.ElementAt(i);

                    float3 newKnotPosition = new float3(
                        continuousDrawSplineTherapist.AnchorPoints[i].transform.position.x,
                        continuousDrawSplineTherapist.AnchorPoints[i].transform.position.y,
                        continuousDrawSplineTherapist.AnchorPoints[i].transform.position.z);

                    knotToMove.Position = newKnotPosition;

                    continuousDrawSplineTherapist.SplineContainer.Spline.SetKnot(i, knotToMove);

                    continuousDrawSplineTherapist.IndexOfLastGrabbedAnchor = i;
                }
            }
        }
    }

    /// <summary>
    /// Used to add knots in edit mode, called by "Add knot" button in Hand Menu
    /// </summary>
    public void AddKnotInEdit()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode)
        {
            // Creates knot GameObject w the given position 
            GameObject knotObj = Instantiate(continuousDrawSplineTherapist.AnchorVisualPrefab);
            knotObj.name = continuousDrawSplineTherapist.AnchorPoints.Count.ToString(); // first knot is named 0, second is 1 (follow collection name convention)
            knotObj.GetComponent<XRGrabInteractable>().enabled = true;

            Vector3 position = new Vector3(
                splineContainerTherapist.Spline.Last().Position.x + 0.1f,
                splineContainerTherapist.Spline.Last().Position.y,
                splineContainerTherapist.Spline.Last().Position.z + 0.1f);

            knotObj.transform.position = position;

            BezierKnot knot = new BezierKnot(position);

            // Adds object to spline and save gameobject    
            splineContainerTherapist.Spline.Add(knot, TangentMode.AutoSmooth);
            continuousDrawSplineTherapist.AnchorPoints.Add(knotObj);
        }

    }

    /// <summary>
    /// Used to delete knots in edit mode, called by "Delete knot" button in Hand Menu
    /// </summary>
    public void DeleteKnot()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode)
        {
            if(continuousDrawSplineTherapist.IndexOfLastGrabbedAnchor > 0 &&
               continuousDrawSplineTherapist.IndexOfLastGrabbedAnchor < continuousDrawSplineTherapist.AnchorPoints.Count)
            {
                // Destroys the knot
                splineContainerTherapist.Spline.RemoveAt(continuousDrawSplineTherapist.IndexOfLastGrabbedAnchor);

                // Destroys the visual game object and removes from list
                Destroy(continuousDrawSplineTherapist.AnchorPoints[continuousDrawSplineTherapist.IndexOfLastGrabbedAnchor]);
                continuousDrawSplineTherapist.AnchorPoints.RemoveAt(continuousDrawSplineTherapist.IndexOfLastGrabbedAnchor);
            }
           
        }
    }

    /// <summary>
    /// Activates and deactivates the spline visual (small black extrusion) drawn by the therapist
    /// </summary>
    public void ToggleSplineExtrude()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode ||
            SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
        {
            if(!splineExtrudeTherapist.enabled)
                splineExtrudeTherapist.enabled = true;
            else 
                splineMeshRenderer.enabled = !splineMeshRenderer.enabled;
        }
    }

    /// <summary>
    /// Activates and deactivates the spline visual (anchor points and dotted path) drawn by the therapist
    /// </summary>
    public void ToggleSplineInstantiate()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode ||
            SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
        {
            if(SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
            {
                continuousDrawSplineTherapist.SetAnchorPointGrabability(false);
            }

            splineInstantiateTherapist.enabled = !splineInstantiateTherapist.enabled;

            //foreach (GameObject anchor in continuousDrawSplineTherapist.AnchorPoints)
            //    anchor.SetActive(!anchor.activeSelf);

            continuousDrawSplineTherapist.ToggleAnchorPointVisibility();
        }
    }

    /// <summary>
    /// Activates and deactivates the spline visual drawn by the patient
    /// </summary>
    public void TogglePlayerSplineInstantiate()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode ||
            SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
        {
            splineInstantiatePlayer.enabled = !splineInstantiatePlayer.enabled;

            continuousDrawSplinePlayer.ToggleAnchorPointVisibility();
        }
    }

    // TODO: figure out how to display radius if not extrusion (daniel?). Maybe copy spline and only have extrusion
    public void ToggleRadius()
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.EditMode ||
            SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
        {
            // TODO: set equal to opposite like above
        }
    }
}
