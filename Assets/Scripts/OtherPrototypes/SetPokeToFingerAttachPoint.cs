using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPokeToFingerAttachPoint : MonoBehaviour
{
    [SerializeField] private Transform pokeAttachPoint;

    private XRPokeInteractor xrPokeInteractor;

    // Start is called before the first frame update
    void Start()
    {
        xrPokeInteractor = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
        SetPokeAttachPoint();
    }

    private void SetPokeAttachPoint()
    {
        if (pokeAttachPoint == null) 
        {
            Debug.Log("Poke Attach Point is null"); 
            return; 
        }

        if (xrPokeInteractor == null)
        {
            Debug.Log("XR Poke Interactor is null");
            return;
        }

        xrPokeInteractor.attachTransform = pokeAttachPoint;
    }
}
