using System.Collections;
using System.Collections.Generic;
using MergeGrid;
using UnityEngine;

public class UpgradesBuyingView : MonoBehaviour
{ 
    [SerializeField] private int _basePrice;
    [SerializeField] private int _additionalPrice;
    [SerializeField] private UpgradeBuyingButton _buyButtonPrefab;
    [SerializeField] private RectTransform _buyButtonsParent;
    [SerializeField] private MergeGridView _mergeGrid;

    private List<UpgradeBuyingButton> _buttons = new List<UpgradeBuyingButton>();
    private Money _money;
    private int _currentProgress;

    public int CurrentPrice => _basePrice + _additionalPrice * _currentProgress;

    public void Init(Upgrade[] upgrades, Money money)
    {
        _currentProgress = UpgradesDataHolder.instance.PriceProgress;
        _money = money;
        _money.Updated += UpdateButtons;
        _mergeGrid.Updated += UpdateButtons;

        foreach (var upgrade in upgrades)
        {
            var newButton = Instantiate(_buyButtonPrefab, _buyButtonsParent);
            newButton.Init(upgrade);

            //calculate price from save
            newButton.UpdatePrice(CurrentPrice);
            newButton.Clicked += OnButtonClicked;
            _buttons.Add(newButton);
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        foreach (var button in _buttons)
        {
            button.SetActive(CurrentPrice <= _money.CurrentMoney && _mergeGrid.HasEmptyCells);
        }
    }

    private void OnButtonClicked(Upgrade upgrade)
    {
        _money.Subtract(CurrentPrice);
        _mergeGrid.AddElement(upgrade);
        _currentProgress++;
        UpgradesDataHolder.instance.SetPriceProgress(_currentProgress);
        UpdateButtons();

        foreach (var upgradeBuyingButton in _buttons)
        {
            upgradeBuyingButton.UpdatePrice(CurrentPrice);
        }
    }
}