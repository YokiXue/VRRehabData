using PathCreation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAlongPath : MonoBehaviour
{
    [SerializeField] private InputActionReference grapInputReference;
    [SerializeField] private GameObject rightHandController;

    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    private float distanceTravelled;
    private float distanceFromControllerToObject;

    void Start()
    {
        //_grapInputReference.action.performed += OnRightGrabPressed;

        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;

            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }

    void Update()
    {
        if (pathCreator != null)
        {
            if (grapInputReference.action.inProgress)
            {   
                // Distance check
                distanceFromControllerToObject = Vector3.Distance(transform.position, rightHandController.transform.position);

                // Cancels grap press so player has to re-press button to continue movement
                if (distanceFromControllerToObject > transform.localScale.y)
                    return;


                // Get the nearest point on the path
                Vector3 nearestPointOnPath = pathCreator.path.GetPointAtDistance(pathCreator.path.GetClosestDistanceAlongPath(rightHandController.transform.position), endOfPathInstruction);

                // Set the draggable object's position to the nearest point on the path
                transform.position = nearestPointOnPath;                
            }
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
