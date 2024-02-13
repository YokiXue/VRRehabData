using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousDrawSpline : MonoBehaviour
{
    // Controller position and inputs
    [Header("Controller")]
    [SerializeField] private InputActionReference grabButtonReference;
    [SerializeField] private InputActionReference triggerButtonReference;
    [SerializeField] private GameObject rightHandController;

    // Creation of splines
    [Header("Splines")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private SplineInstantiate splineInstantiate;

    // Used for visualising anchorpoints
    [Header("Visuals")]
    [SerializeField] private GameObject anchorVisualPrefab;
    [SerializeField] private float spawnDelay = 1;

    /// <summary>
    /// Collections of all visible anchorpoints
    /// </summary>
    public List<GameObject> AnchorPoints { get; set; } = new List<GameObject>();
    
    /// <summary>
    /// Access to the spline container holding the therapists drawn path
    /// </summary>
    public SplineContainer SplineContainer { get => splineContainer; set => splineContainer = value; }
    /// <summary>
    /// Access to the serialized field containing the Anchor Visual
    /// </summary>
    public GameObject AnchorVisualPrefab { get => anchorVisualPrefab; set => anchorVisualPrefab = value; }

    ///// <summary>
    ///// The current state of the spline, ie draw/edit/play
    ///// </summary>
    //public SplineDrawState CurrentSplineState { get; set; }

    /// <summary>
    /// Saves the anchor that was last grapped by the user
    /// </summary>
    public int IndexOfLastGrabbedAnchor { get; set; }

    public float TimeTaken { get; set; }

    private float nextSpawn;
    private bool drawSpline = false;
    private bool startTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        //if(SplineStateManager.CurrentSplineState == null)
        //    SplineStateManager.CurrentSplineState = SplineDrawState.DrawMode;

        grabButtonReference.action.started += OnRightGrabPressed;
        grabButtonReference.action.canceled += OnRightGrabReleased;
    }

    private void FixedUpdate()
    {
        // Spawns anchorpoints while grab is pressed (with delay)
        // For player, change so that it only starts drawing after they make contact w trajectory for the first time
        if (drawSpline)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnDelay;
                AddKnotInDraw(rightHandController.transform.position);
            }
        }
        if (startTimer)
        {
            TimeTaken += Time.deltaTime;
        }
    }

    private void AddKnotInDraw(Vector3 position)
    {
        // Creates knot GameObject w the given position 
        GameObject knotObj = Instantiate(anchorVisualPrefab);
        knotObj.name = AnchorPoints.Count.ToString(); // first knot is named 0, second is 1 (follow collection name convention)
        knotObj.transform.position = position;

        BezierKnot knot = new BezierKnot(position);

        // Adds object to spline and save transform
        splineContainer.Spline.Add(knot, TangentMode.AutoSmooth);
        AnchorPoints.Add(knotObj);
    }

    public void SetAnchorPointVisibility(bool setActive)
    {
        splineInstantiate.enabled = setActive;

        foreach (GameObject anchor in AnchorPoints)
        {
            anchor.GetComponent<MeshRenderer>().enabled = setActive;
        }
    }

    public void ToggleAnchorPointVisibility()
    {
        foreach (GameObject anchor in AnchorPoints)
        {
            MeshRenderer mr = anchor.GetComponent<MeshRenderer>();
            mr.enabled = !mr.enabled;
        }
    }

    public void SetAnchorPointGrabability(bool setActive)
    {
        foreach (GameObject anchor in AnchorPoints)
        {
            anchor.GetComponent<XRGrabInteractable>().enabled = setActive;
        }
    }


    /// <summary>
    /// Called when going from draw mode to edit mode <para>
    /// Creates a new spline with extrusion component</para>
    /// </summary>
    public void CopySplineToVisualizeRadius()
    {
        // TODO: actually do it lol, maybe move to editspline class
    }

    #region Input Actions
    private void OnRightGrabReleased(InputAction.CallbackContext obj)
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.DrawMode || 
            SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
            drawSpline = false;
        if (SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
            startTimer = false;
    }

    /// <summary>
    /// Spawns a knot when user presses trigger button
    /// </summary>
    /// <param name="obj"></param>
    private void OnRightGrabPressed(InputAction.CallbackContext obj)
    {
        if (SplineStateManager.CurrentSplineState == SplineDrawState.DrawMode ||
            SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
            drawSpline = true;
        if (SplineStateManager.CurrentSplineState == SplineDrawState.PlayMode)
            startTimer = true;
    }

    #endregion
}
