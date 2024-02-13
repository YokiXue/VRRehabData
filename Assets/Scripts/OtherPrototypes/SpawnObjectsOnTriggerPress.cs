using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnObjectsOnTriggerPress : MonoBehaviour
{
    /// <summary>
    /// Input Action reference to the trigger button, to track when pressed
    /// </summary>
    [SerializeField] private InputActionReference triggerInputActionReference;
    /// <summary>
    /// A prefab of the object that gets spawned when pressing trigger (used for visuals)
    /// </summary>
    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private TMP_Text distanceText;

    /// <summary>
    /// Collection of the two objects spawned by player
    /// </summary>
    private List<GameObject> spawnedObjects = new List<GameObject>();

    /// <summary>
    /// Check for if both start/end object has been placed
    /// </summary>
    private bool completeMeasurement = false;

    /// <summary>
    /// The ingame distance between the two objects placed by player
    /// </summary>
    private float distanceBetweenObjects;

    private void Start()
    {
        triggerInputActionReference.action.performed += OnTriggerPress;
    }

    private void OnTriggerPress(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            GameObject objTS = Instantiate(objectToSpawn);
            objTS.transform.position = transform.position;
            spawnedObjects.Add(objTS);

            Debug.Log("Added new object nr " + spawnedObjects.Count);

            if (spawnedObjects.Count == 2)
                completeMeasurement = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If player has pressed Trigger button twice
        if (completeMeasurement)
        {
            // Uses Vector3.Distance (euclidean distance) to measure distance between the two objects places by player
            // Could be saved in singleton scriptable object in future / test in this prototype?
            distanceBetweenObjects = Vector3.Distance(spawnedObjects[0].transform.position, spawnedObjects[1].transform.position);

            Debug.Log("Distance measured between objects is: " + distanceBetweenObjects);

            distanceText.text = distanceBetweenObjects.ToString();
            uiCanvas.SetActive(true);

            completeMeasurement = false;
            spawnedObjects.Clear();
        }

    }
}
