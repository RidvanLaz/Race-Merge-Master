using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTextRotater : MonoBehaviour
{
    public Transform _lookTarget;

    private void Update()
    {
        transform.LookAt(_lookTarget);
    }
}
