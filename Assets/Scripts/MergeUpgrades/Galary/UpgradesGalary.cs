using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class UpgradesGalary : MonoBehaviour
{
    [SerializeField] private UpgradeGalaryView[] _galaries;

    public void Init(Upgrade[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            _galaries[i].CreateViews(upgrades[i]);
        }
    }
}