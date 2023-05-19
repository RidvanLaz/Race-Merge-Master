using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesView : MonoBehaviour
{
    [SerializeField] private UpgradeSliderView _sliderPrefab;
    [SerializeField] private RectTransform _sliderParent;

    private List<UpgradeSliderView> _sliderViews = new List<UpgradeSliderView>();

    public void Init(Upgrade[] upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            var newSlider = Instantiate(_sliderPrefab, _sliderParent);
            newSlider.Init(upgrade);
            _sliderViews.Add(newSlider);
        }
    }
}