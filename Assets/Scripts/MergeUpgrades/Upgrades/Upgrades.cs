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

    [Space] 
    [SerializeField] private CarKuzovUpgrade _carKuzovUpgrade;

    public Upgrade[] UpgradesList => _upgrades;

    private Money _money;

    private IEnumerator Start()
    {
        yield return null;

        _money = new Money(PointsTransmitter.Instance.GetWalletPoints());

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

        _carKuzovUpgrade.Init(_upgrades[0], _upgrades[2]);
    }

    private void UpdateKuzovUpgrade(int newLevel)
    {
        UpgradesDataHolder.instance.SaveKuzovUpgrade(newLevel);
    }

    private void UpdateEngineUpgrade(int newLevel)
    {
        UpgradesDataHolder.instance.SaveEngineUpgrade(newLevel);
    }
    private void UpdateWheelsUpgrade(int newLevel)
    {
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
        SceneManager.LoadScene(UpgradesDataHolder.instance.NextSceneIndex);
    }
}