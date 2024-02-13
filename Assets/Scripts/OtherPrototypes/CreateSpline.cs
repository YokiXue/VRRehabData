using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using TreeEditor;

public class CreateSpline : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private InputActionReference grabButtonReference;
    [SerializeField] private InputActionReference triggerButtonReference;
    [SerializeField] private GameObject rightHandController;

    [Header("Splines")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private SplineInstantiate splineInstantiate;
    
    [Header("Visuals")]
    [SerializeField] private Material sphereMaterial;

    public List<Transform> KnotTransform { get; set; } = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        grabButtonReference.action.performed += OnRightGrabPressed;
        triggerButtonReference.action.performed += OnRightTriggerPressed;
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext obj)
    {
        if(obj.performed)
        {
            splineInstantiate.enabled = !splineInstantiate.enabled;
        }
    }

    /// <summary>
    /// Spawns a knot when user presses trigger button
    /// </summary>
    /// <param name="obj"></param>
    private void OnRightGrabPressed(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            AddKnot(rightHandController.transform.position);    
        }
    }

    private void AddKnot(Vector3 position)
    {
        // Creates knot GameObject w the given position 
        GameObject knotObj = new GameObject("Knot");
        knotObj.transform.position = position;

        // Adds object to spline and save transform
        splineContainer.Spline.Add(new BezierKnot(knotObj.transform.position), TangentMode.AutoSmooth); 
        KnotTransform.Add(knotObj.transform);

        VisualizeKnots();

    }

    private void VisualizeKnots()
    {
        foreach(Transform tm in KnotTransform)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = tm.position;
            sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            sphere.GetComponent<MeshRenderer>().material = sphereMaterial;
        }
    }
}
