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
    [SerializeField] private UpgradesGalary _galary;
    [SerializeField] private UpgradesFXReaction _upgradeFX;

    [Space] 
    [SerializeField] private CarKuzovUpgrade _carKuzovUpgrade;

    public Upgrade[] UpgradesList => _upgrades;

    private Money _money;

    private void Start()
    {
        _money = new Money(100000);

        _moneyView.Init(_money);
        _mergeGrid.Init(_upgrades, _money);
        _upgradesBuyingView.Init(_upgrades, _money);
        _upgradesView.Init(_upgrades);
        _galary.Init(_upgrades);
        _upgradeFX.Init(_upgrades);

        _carKuzovUpgrade.Init(_upgrades[0], _upgrades[2]);
    }

    [EditorButton]
    public void AddMoney()
    {
        _money.AddMoney(1000);
    }
}