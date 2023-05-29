using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuyingButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private GameObject _blockingMask;
    [SerializeField] private GameObject _priceObj;
    [SerializeField] private GameObject _adObj;

    private Upgrade _upgrade;
    private bool _isForAd;

    public event Action<Upgrade, bool> Clicked;

    public void Init(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _icon.sprite = upgrade.GetSpriteForLevel(1);
    }

    public void UpdatePrice(int newPrice)
    {
        _price.text = newPrice.ToString();
    }

    public void SetActive(bool isActive, bool hasSpace, bool canAd)
    {
        _blockingMask.SetActive(hasSpace == false || (isActive == false && canAd == false));
        _isForAd = isActive == false && canAd;
        _priceObj.SetActive(isActive || canAd == false);
        _adObj.SetActive(isActive == false && canAd);
    }

    public void Click()
    {
        Clicked?.Invoke(_upgrade, _isForAd);
    }
}