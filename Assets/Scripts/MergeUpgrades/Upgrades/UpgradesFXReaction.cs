using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesFXReaction : MonoBehaviour
{
    [SerializeField] private ParticleSystem _carUpgradeFX;

    private Upgrade[] _upgrades;

    public void Init(Upgrade[] upgrades)
    {
        _upgrades = upgrades;

        foreach (var upgrade in _upgrades)
        {
            upgrade.LevelIncreased += PlayFX;
        }
    }

    private void PlayFX(int newLevel)
    {
        _carUpgradeFX.Play();
    }

    private void OnDestroy()
    {
        foreach (var upgrade in _upgrades)
        {
            upgrade.LevelIncreased -= PlayFX;
        }
    }
}
