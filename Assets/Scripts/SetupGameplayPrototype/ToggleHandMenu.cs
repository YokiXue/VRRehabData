using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleHandMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference leftTriggerActionReference;
    [SerializeField] private GameObject handObject;

    private void Start()
    {
        handObject.SetActive(false);

        leftTriggerActionReference.action.started += OnLeftTriggerStarted;
        leftTriggerActionReference.action.canceled += OnLeftTriggerCancelled;
    }

    private void OnLeftTriggerCancelled(InputAction.CallbackContext obj)
    {
        handObject.SetActive(false);
    }

    private void OnLeftTriggerStarted(InputAction.CallbackContext obj)
    {
        handObject.SetActive(true);
    }
}
