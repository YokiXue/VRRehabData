using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragFreely : MonoBehaviour
{
    [SerializeField] private InputActionReference grapInputReference;
    [SerializeField] private GameObject rightHandController;

    private float distanceFromControllerToObject;

    // Start is called before the first frame update
    void Start()
    {
        grapInputReference.action.performed += OnGrabPressed;
    }

    private void Update()
    {
        if (grapInputReference.action.inProgress)
        {
            // Distance check
            distanceFromControllerToObject = Vector3.Distance(transform.position, rightHandController.transform.position);

            // Cancels grap press so player has to re-press button to continue movement
            if (distanceFromControllerToObject > transform.localScale.y)
                return;

            transform.position = rightHandController.transform.position;
        }
    }
    private void OnGrabPressed(InputAction.CallbackContext context)
    {
        
    }

}
