using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projector : MonoBehaviour
{
    public Car Car;

    private SplineProjector _splineProjector;

    private void Awake()
    {
        _splineProjector = GetComponent<SplineProjector>();
        _splineProjector.projectTarget = Car.transform;
    }

    public Car GetCar()
    {
        return Car;
    }
}
