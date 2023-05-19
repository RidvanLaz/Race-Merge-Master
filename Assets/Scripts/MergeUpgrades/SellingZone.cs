using System.Collections;
using System.Collections.Generic;
using MergeGrid;
using TMPro;
using UnityEngine;

public class SellingZone : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _zone;
    [SerializeField] private TMP_Text _sellCost;
    [SerializeField] private int _costForLevel = 65;

    private Money _money;

    public void Init(Money money)
    {
        _money = money;
    }

    public void Open(MergeGridElement gridElement)
    {
        gameObject.SetActive(true);
        _sellCost.text = $"Sell: + {(gridElement.Level * _costForLevel)}";
    }

    public void Sell(MergeGridElement gridElement)
    {
        //add sound
        _money.AddMoney(gridElement.Level * _costForLevel);
    }

    public bool IsInZone(Vector2 position)
    {
        bool isInX = position.x <= _zone.position.x + _zone.sizeDelta.x * _canvas.transform.localScale.x * 0.5f
                     && position.x >= _zone.position.x - _zone.sizeDelta.x * _canvas.transform.localScale.x * 0.5f;

        bool isInY = position.y <= _zone.position.y + _zone.sizeDelta.y * _canvas.transform.localScale.x * 0.5f
                     && position.y >= _zone.position.y - _zone.sizeDelta.y * _canvas.transform.localScale.x * 0.5f;

        return isInY && isInX;
    }
}
