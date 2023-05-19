using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MergeGrid;

public class Upgrades : MonoBehaviour
{
    [SerializeField] private Upgrade[] _upgrades;
    [SerializeField] private MergeGridView _mergeGrid;
    [SerializeField] private UpgradesBuyingView _upgradesBuyingView;
    [SerializeField] private MoneyView _moneyView;
    [SerializeField] private UpgradesView _upgradesView;

    [Space] 
    [SerializeField] private CarKuzovUpgrade _carKuzovUpgrade;

    public Upgrade[] UpgradesList => _upgrades;

    private Money _money;

    private void Start()
    {
        _money = new Money(1000);

        _moneyView.Init(_money);
        _mergeGrid.Init(_upgrades, _money);
        _upgradesBuyingView.Init(_upgrades, _money);
        _upgradesView.Init(_upgrades);

        _carKuzovUpgrade.Init(_upgrades[0]);
    }

    [EditorButton]
    public void AddMoney()
    {
        _money.AddMoney(1000);
    }
}