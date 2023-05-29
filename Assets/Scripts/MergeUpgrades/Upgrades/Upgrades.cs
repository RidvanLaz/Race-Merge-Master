using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MergeGrid;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(100000)]
public class Upgrades : MonoBehaviour
{
    [SerializeField] private Upgrade[] _upgrades;
    [SerializeField] private MergeGridView _mergeGrid;
    [SerializeField] private UpgradesBuyingView _upgradesBuyingView;
    [SerializeField] private MoneyView _moneyView;
    [SerializeField] private UpgradesView _upgradesView;
    [SerializeField] private UpgradesGalary _galary;
    [SerializeField] private UpgradesFXReaction _upgradeFX;
    [SerializeField] private AnalyticManager _analytic;
    [SerializeField] private MergeTutorial _mergeTutorial;
    [SerializeField] private Data _data;

    [Space] 
    [SerializeField] private CarKuzovUpgrade _carKuzovUpgrade;

    public Upgrade[] UpgradesList => _upgrades;

    private Money _money;

    private IEnumerator Start()
    {
        yield return null;

        if (_data.Options.IsTutorial)
        {
            var neededMoney = 0;

            for (int i = 0; i < _upgrades.Length; i++)
            {
                neededMoney += _upgradesBuyingView.BasePrice + _upgradesBuyingView.AdditionalPrice * i;
            }

            _money = new Money(PointsTransmitter.Instance.GetWalletPoints() + neededMoney);
        }
        else
        {
            _money = new Money(PointsTransmitter.Instance.GetWalletPoints());
        }

        _upgrades[0].Init(UpgradesDataHolder.instance.GetKuzovUpgrade() - 1);
        _upgrades[1].Init(UpgradesDataHolder.instance.GetEngineUpgrade() - 1);
        _upgrades[2].Init(UpgradesDataHolder.instance.GetWheelsUpgrade() - 1);

        _upgrades[0].LevelChanged += UpdateKuzovUpgrade;
        _upgrades[1].LevelChanged += UpdateEngineUpgrade;
        _upgrades[2].LevelChanged += UpdateWheelsUpgrade;

        _moneyView.Init(_money);
        _mergeGrid.Init(_upgrades, _money);
        _upgradesBuyingView.Init(_upgrades, _money);
        _upgradesView.Init(_upgrades);
        _galary.Init(_upgrades);
        _upgradeFX.Init(_upgrades);
        _mergeTutorial.Init(_upgrades, _mergeGrid);

        _carKuzovUpgrade.Init(_upgrades[0], _upgrades[2]);
    }

    private void UpdateKuzovUpgrade(int newLevel)
    {
        _analytic.SendUpgradeEvent("Kuzov");
        UpgradesDataHolder.instance.SaveKuzovUpgrade(newLevel);
    }

    private void UpdateEngineUpgrade(int newLevel)
    {
        _analytic.SendUpgradeEvent("Engine");
        UpgradesDataHolder.instance.SaveEngineUpgrade(newLevel);
    }
    private void UpdateWheelsUpgrade(int newLevel)
    {
        _analytic.SendUpgradeEvent("Wheels");
        UpgradesDataHolder.instance.SaveWheelsUpgrade(newLevel);
    }

    private void OnDestroy()
    {
        if (PointsTransmitter.Instance != null)
            PointsTransmitter.Instance.DropCollectedPoints(_money.CurrentMoney);
    }

    [EditorButton]
    public void AddMoney()
    {
        _money.AddMoney(1000);
    }

    public void LoadNextLevel()
    {
        if (_data.Options.IsTutorial)
        {
            _data.Options.IsTutorial = false;
            _data.Save();
        }

        SceneManager.LoadScene(UpgradesDataHolder.instance.NextSceneIndex);
    }
}