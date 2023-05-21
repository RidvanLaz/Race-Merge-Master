using System.Collections.Generic;
using UnityEngine;

public class UpgradeGalaryView : MonoBehaviour
{
    [SerializeField] private UpgradeLevelView _levelViewPrefab;
    [SerializeField] private RectTransform _levelViewsParent;

    private List<UpgradeLevelView> _levelViews = new List<UpgradeLevelView>();
    private Upgrade _upgrade;

    public void CreateViews(Upgrade upgrade)
    {
        _upgrade = upgrade;
        upgrade.LevelChanged += UpdateViews;
        for (int i = 0; i < upgrade.MaxLevel; i++)
        {
            var newView = Instantiate(_levelViewPrefab, _levelViewsParent);
            _levelViews.Add(newView);
            newView.Init(upgrade.GetSpriteForLevel(i + 1), i + 1, i + 1 <= upgrade.CurrentLevel);
        }
    }

    private void UpdateViews(int newCurrentLevel)
    {
        for (int i = 0; i < _levelViews.Count; i++)
        {
            _levelViews[i].Init(_upgrade.GetSpriteForLevel(i + 1), i + 1, i + 1 <= newCurrentLevel);
        }
    }

    private void OnDestroy()
    {
        if (_upgrade != null)
            _upgrade.LevelChanged -= UpdateViews;
    }
}