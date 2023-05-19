using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuyingButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private GameObject _blockingMask;

    private Upgrade _upgrade;

    public event Action<Upgrade> Clicked;

    public void Init(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _icon.sprite = upgrade.GetSpriteForLevel(1);
    }

    public void UpdatePrice(int newPrice)
    {
        _price.text = newPrice.ToString();
    }

    public void SetActive(bool isActive)
    {
        _blockingMask.SetActive(isActive == false);
    }

    public void Click()
    {
        Clicked?.Invoke(_upgrade);
    }
}