using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLocker : MonoBehaviour
{
    [Range(20, 180)][SerializeField] private int _targetFPS = 60;

    private void Start()
    {
        Application.targetFrameRate = _targetFPS;
    }
}
