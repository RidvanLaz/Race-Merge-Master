using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MergeGrid;
using UnityEngine;

public class UpgradesBuyingView : MonoBehaviour
{ 
    [SerializeField] private int _basePrice;
    [SerializeField] private int _additionalPrice;
    [SerializeField] private UpgradeBuyingButton _buyButtonPrefab;
    [SerializeField] private RectTransform _buyButtonsParent;
    [SerializeField] private MergeGridView _mergeGrid;
    [SerializeField] private Data _data;

    private List<UpgradeBuyingButton> _buttons = new List<UpgradeBuyingButton>();
    private Money _money;
    private int _currentProgress;
    private bool[] _canAds;
    private bool _isTutorial;
    private List<Upgrade> _upgrades = new List<Upgrade>();

    public int BasePrice => _basePrice;
    public int AdditionalPrice => _additionalPrice;

    public int CurrentPrice => _basePrice + _additionalPrice * _currentProgress;

    public void Init(Upgrade[] upgrades, Money money)
    {
        _isTutorial = _data.Options.IsTutorial;
        _canAds = new bool[upgrades.Length];

        for (int i = 0; i < _canAds.Length; i++)
        {
            _canAds[i] = true;
        }

        _upgrades = new List<Upgrade>(upgrades.ToList());
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
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetActive(CurrentPrice <= _money.CurrentMoney, _mergeGrid.HasEmptyCells, _canAds[i] && Ads.IsRewardedAdReady() && _isTutorial == false);
        }
    }

    private void Update()
    {
        UpdateButtons();
    }

    private void OnButtonClicked(Upgrade upgrade, bool isForAd)
    {
        if (isForAd && Ads.IsRewardedAdReady())
        {
            Ads.ShowRewarded("", result =>
            {
                if (result == AdResult.Finished)
                {
                    _canAds[_upgrades.IndexOf(upgrade)] = false;
                    StartCoroutine(UpgradeWithPause(upgrade));
                }
            });
        }
        else
        {
            _money.Subtract(CurrentPrice);
            _currentProgress++;
            BuyNewUpgradeElement(upgrade);
        }
    }

    private void BuyNewUpgradeElement(Upgrade upgrade)
    {
        _mergeGrid.AddElement(upgrade);
        UpgradesDataHolder.instance.SetPriceProgress(_currentProgress);
        UpdateButtons();

        foreach (var upgradeBuyingButton in _buttons)
        {
            upgradeBuyingButton.UpdatePrice(CurrentPrice);
        }
    }

    private IEnumerator UpgradeWithPause(Upgrade upgrade)
    {
        yield return new WaitForSeconds(0.1f);
        BuyNewUpgradeElement(upgrade);
    }
}