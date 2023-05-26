using Dreamteck;
using MergeGrid;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue + 1)]
public class UpgradesDataHolder : Singleton<UpgradesDataHolder>
{
    [SerializeField] private Data _data;

    private void Awake()
    {
        _data.Load();
        PointsTransmitter.Instance.InitData(_data);
        PointsTransmitter.Instance.SetPoints(_data.GetCurrentSoft());
    }

    public int GetEngineUpgrade() => _data.Options.EngineUpgrade;
    public int GetKuzovUpgrade() => _data.Options.KuzovUpgrade;
    public int GetWheelsUpgrade() => _data.Options.WheelsUpgrade;

    public int NextSceneIndex => _data.GetLevelIndex();
    public int PriceProgress => _data.Options.PriceProgress;

    public MergeGridData GetGridData() => _data.Options.MergeGridData;

    public void SaveEngineUpgrade(int upgrade)
    {
        _data.Options.EngineUpgrade = upgrade;
        _data.Save();
    }

    public void SaveKuzovUpgrade(int upgrade)
    {
        _data.Options.KuzovUpgrade = upgrade;
        _data.Save();
    }

    public void SaveWheelsUpgrade(int upgrade)
    {
        _data.Options.WheelsUpgrade = upgrade;
        _data.Save();
    }

    public void SaveGrid(MergeGridData data)
    {
        _data.Options.MergeGridData = data;
        _data.Save();
    }

    public void SetPriceProgress(int progress)
    {
        _data.Options.PriceProgress = progress;
        _data.Save();
    }
}