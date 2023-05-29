using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private GameObject _moveTutorial;
    [SerializeField] private VariableJoystick _joystick;

    private void Start()
    {
        if (_data.Options.IsTutorial)
        {
            _moveTutorial.SetActive(true);
            _joystick.PointerDown += HideTutorial;
        }
    }

    private void OnDestroy()
    {
        if (_joystick != null)
            _joystick.PointerDown -= HideTutorial;
    }

    private void HideTutorial()
    {
        _joystick.PointerDown -= HideTutorial;
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return null;
        _moveTutorial.SetActive(false);
    }
}
