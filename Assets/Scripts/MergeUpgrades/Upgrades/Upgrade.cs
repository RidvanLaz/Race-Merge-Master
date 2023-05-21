using System;
using UnityEngine;

[Serializable]
public class Upgrade
{
    [Serializable]
    public class Level
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _value;

        public Sprite Icon => _icon;
        public float Value => _value;
    }

    [SerializeField] private Level[] _levels;
    [SerializeField] private string _name;

    private int _currentLevel;

    public event Action<int> LevelChanged;
    public event Action<int> LevelIncreased;

    public string Name => _name;

    public void Init(int currentLevel)
    {
        if (currentLevel < 0 || currentLevel >= _levels.Length)
            throw new ArgumentException($"{nameof(currentLevel)} can't be less than 0 and bigger than max level");

        _currentLevel = currentLevel;
    }

    public bool SetLevel(int level)
    {
        var oldLevel = _currentLevel;
        _currentLevel = Mathf.Clamp(level - 1, 0, MaxLevel - 1);
        LevelChanged?.Invoke(_currentLevel + 1);

        if (oldLevel < _currentLevel)
            LevelIncreased?.Invoke(_currentLevel);
        return true;
    }

    public int CurrentLevel => _currentLevel + 1;
    public Sprite CurrentLevelIcon => _levels[_currentLevel].Icon;
    public float CurrentLevelValue => _levels[_currentLevel].Value;
    public int MaxLevel => _levels.Length;
    public float Progress => (float)(_currentLevel + 1) / (float)_levels.Length;
    public bool IsMaxed => CurrentLevel == MaxLevel;

    public Sprite GetSpriteForLevel(int level)
    {
        if (level < 1 || level > _levels.Length)
            throw new ArgumentException($"{nameof(level)} can't be less than 0 and bigger than max level");

        return _levels[level - 1].Icon;
    }
}