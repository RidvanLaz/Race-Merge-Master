using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarKuzovUpgrade : MonoBehaviour
{
    [Serializable]
    public class CarKuzov
    {
        public GameObject Kuzov;
        public Transform[] WheelsPlaces;
    }

    [Serializable]
    public class WheelsHolder
    {
        public Transform[] Wheels;
    }

    [SerializeField] private CarKuzov[] _kuzovs;
    [SerializeField] private WheelsHolder[] _wheels;

    private Upgrade _kuzovUpgrade;
    private Upgrade _wheelsUpgrade;
    private int _currentWheelsIndex;

    public void Init(Upgrade kuzovUpgrade, Upgrade wheelsUpgrade)
    {
        _kuzovUpgrade = kuzovUpgrade;
        _wheelsUpgrade = wheelsUpgrade;
        _kuzovUpgrade.LevelChanged += OnUpgraded;
        _wheelsUpgrade.LevelChanged += PlaceWheels;
        OnUpgraded(_kuzovUpgrade.CurrentLevel);
        PlaceWheels(_wheelsUpgrade.CurrentLevel);
    }

    public void PlaceWheels(int wheelsLevel)
    {
        _currentWheelsIndex = wheelsLevel - 1;

        for (int i = 0; i < _wheels.Length; i++)
        {
            for (int j = 0; j < _wheels[i].Wheels.Length; j++)
            {
                _wheels[i].Wheels[j].gameObject.SetActive(i == _currentWheelsIndex);
            }
        }

        var targetKuzov = _kuzovs[_kuzovUpgrade.CurrentLevel - 1];

        for (int i = 0; i < targetKuzov.WheelsPlaces.Length; i++)
        {
            _wheels[_currentWheelsIndex].Wheels[i].parent = targetKuzov.WheelsPlaces[i];
            _wheels[_currentWheelsIndex].Wheels[i].localPosition = Vector3.zero;
            _wheels[_currentWheelsIndex].Wheels[i].localEulerAngles = Vector3.zero;
        }

    }

    private void OnDestroy()
    {
        _kuzovUpgrade.LevelChanged -= OnUpgraded;
        _wheelsUpgrade.LevelChanged -= PlaceWheels;
    }

    private void OnUpgraded(int newLevel)
    {
        for (int i = 0; i < _kuzovs.Length; i++)
        {
            _kuzovs[i].Kuzov.SetActive(newLevel == i + 1);
        }
        
        PlaceWheels(_currentWheelsIndex + 1);
    }
}
