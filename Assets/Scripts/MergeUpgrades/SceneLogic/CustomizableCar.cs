using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizableCar : MonoBehaviour
{
    [Serializable]
    public class WheelsHolder
    {
        public Transform[] Wheels;
    }

    [SerializeField] private Transform _cameraAnchor;
    [SerializeField] private FloatinTextHandler _floatingTextHandler;
    [SerializeField] private NameTextRotater _nameTextRotater;
    [SerializeField] private WheelsHolder[] _wheels;

    public Transform CameraAnchor => _cameraAnchor;
    public FloatinTextHandler FloatingTextHandler => _floatingTextHandler;
    public NameTextRotater NameTextRotater => _nameTextRotater;

    public void ActivateWheels(int index)
    {
        for (int i = 0; i < _wheels.Length; i++)
        {
            foreach (var wheel in _wheels[i].Wheels)
            {
                wheel.gameObject.SetActive(i == index);
            }
        }
    }
}
