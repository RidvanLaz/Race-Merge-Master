using System;
using UnityEngine;

[Serializable]
public class Money
{
    [SerializeField] private int _currentMoney;

    public int CurrentMoney => _currentMoney;

    public event Action Updated;

    public Money(int currentMoney)
    {
        _currentMoney = currentMoney;
    }

    public void AddMoney(int money)
    {
        if (money <= 0)
            throw new ArgumentException($"{nameof(money)} should be positive");

        _currentMoney += money;
        Updated?.Invoke();
    }

    public void Subtract(int money)
    {
        if (money <= 0)
            throw new ArgumentException($"{nameof(money)} should be positive");

        if (money > CurrentMoney)
            throw new ArgumentException($"{nameof(money)} should be less than {nameof(CurrentMoney)}");

        _currentMoney -= money;
        Updated?.Invoke();
    }
}