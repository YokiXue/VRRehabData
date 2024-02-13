using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class UpdateSpline : MonoBehaviour
{
    [SerializeField] private GameObject spline;
    [SerializeField] private Slider slider;

    private SplineExtrude splineExtrude;
    private Vector3 scale;

    private void Start()
    {
        splineExtrude = spline.GetComponent<SplineExtrude>();

        splineExtrude.RebuildOnSplineChange = true;
    }

    public void OnRadiusSliderChanged()
    {
        splineExtrude.Radius = slider.value / 4;
        splineExtrude.Rebuild();
    }

    public void OnScaleSliderChanged()
    {
        spline.transform.localScale = new Vector3(slider.value, slider.value, slider.value);
    }
}
