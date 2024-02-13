using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    /// <summary>
    /// Struct for saving spline data, rn only works for one data
    /// </summary>
    public struct SplineData
    {
        public int SplineNumber;
        public int Radius;
        public List<Vector3> KnotPositions;
    }


    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
