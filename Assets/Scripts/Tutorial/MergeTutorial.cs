using System.Collections;
using System.Collections.Generic;
using MergeGrid;
using UnityEngine;

public class MergeTutorial : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private GameObject _firstBuyTutorial;
    [SerializeField] private GameObject _secondBuyTutorial;
    [SerializeField] private GameObject _thirdBuyTutorial;

    [Space]
    [SerializeField] private MergeVisualTutorial _mergeVisualTutorial;

    [Space]
    [SerializeField] private GameObject _finalButton;
    [SerializeField] private GameObject _buttonPressTutorial;

    private Upgrade[] _upgrades;
    private MergeGridView _mergeGrid;
    private Upgrade _currentUpgradeToMerge;

    public Upgrade CurrentUpgradeToMerge => _currentUpgradeToMerge;

    public void Init(Upgrade[] upgrades, MergeGridView mergeGrid)
    {
        _upgrades = upgrades;
        _mergeGrid = mergeGrid;

        if (_data.Options.IsTutorial)
        {
            _firstBuyTutorial.SetActive(true);
            _mergeGrid.Updated += PassFirstBuy;
            _finalButton.SetActive(false);
        }
    }

    private void PassFirstBuy()
    {
        if (_mergeGrid.CurrentElementsAmount == 4)
        {
            _mergeGrid.Updated -= PassFirstBuy;
            _firstBuyTutorial.SetActive(false);
            _secondBuyTutorial.SetActive(true);
            _mergeGrid.Updated += PassSecondBuy;
        }
    }

    private void PassSecondBuy()
    {
        if (_mergeGrid.CurrentElementsAmount == 5)
        {
            _mergeGrid.Updated -= PassSecondBuy;
            _secondBuyTutorial.SetActive(false);
            _thirdBuyTutorial.SetActive(true);
            _mergeGrid.Updated += PassThirdBuy;
        }
    }

    private void PassThirdBuy()
    {
        if (_mergeGrid.CurrentElementsAmount == 6)
        {
            _mergeGrid.Updated -= PassThirdBuy;
            _currentUpgradeToMerge = _upgrades[0];
            _thirdBuyTutorial.SetActive(false);

            _mergeVisualTutorial.gameObject.SetActive(true);
            _mergeVisualTutorial.Init(_mergeGrid);
            _mergeVisualTutorial.SetTargetUpgrade(_upgrades[0]);

            _upgrades[0].LevelIncreased += FirstMergePass;
        }
    }

    private void FirstMergePass(int newLevel)
    {
        _upgrades[0].LevelIncreased -= FirstMergePass;
        _currentUpgradeToMerge = _upgrades[1];
        _mergeVisualTutorial.SetTargetUpgrade(_upgrades[1]);
        _upgrades[1].LevelIncreased += SecondMergePass;
    }

    private void SecondMergePass(int newLevel)
    {
        _upgrades[1].LevelIncreased -= SecondMergePass;
        _currentUpgradeToMerge = _upgrades[2];
        _mergeVisualTutorial.SetTargetUpgrade(_upgrades[2]);
        _upgrades[2].LevelIncreased += ThirdMergePass;
    }

    private void ThirdMergePass(int newLevel)
    {
        _finalButton.SetActive(true);
        _upgrades[2].LevelIncreased -= ThirdMergePass;
        _mergeVisualTutorial.Stop();
        _buttonPressTutorial.SetActive(true);
    }
}
