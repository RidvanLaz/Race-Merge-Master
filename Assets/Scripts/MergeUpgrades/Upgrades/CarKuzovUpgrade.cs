using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarKuzovUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject[] _kuzovs;

    private Upgrade _upgrade;

    public void Init(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _upgrade.LevelChanged += OnUpgraded;
        OnUpgraded(upgrade.CurrentLevel);
    }

    private void OnDestroy()
    {
        _upgrade.LevelChanged -= OnUpgraded;
    }

    private void OnUpgraded(int newLevel)
    {
        for (int i = 0; i < _kuzovs.Length; i++)
        {
            _kuzovs[i].SetActive(newLevel == i + 1);
        }
    }
}
