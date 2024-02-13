using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class GetPlayerObjectSpawnPosition : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private bool setFirst;

    // Start is called before the first frame update
    void Start()
    {
        if(setFirst)
            transform.position = splineContainer.Spline.Knots.First().Position;
        else
            transform.position = splineContainer.Spline.Knots.Last().Position;
    }
}
