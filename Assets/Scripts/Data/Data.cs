using System;
using MergeGrid;
using UnityEngine;

public class Data : MonoBehaviour
{
    protected const string _dataKeyName = "AutoAccident";
    protected SaveOptions _options = new SaveOptions();

    public void Save()
    {
        string json = JsonUtility.ToJson(_options);
        PlayerPrefs.SetString(_dataKeyName, json);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(_dataKeyName) == false)
        {
            _options = new SaveOptions();
            Save();
        }
        else
            _options = JsonUtility.FromJson<SaveOptions>(PlayerPrefs.GetString(_dataKeyName));

        if (_options.IsTutorial)
        {
            _options.Soft = 0;
            _options.KuzovUpgrade = 1;
            _options.EngineUpgrade = 1;
            _options.WheelsUpgrade = 1;
            _options.PriceProgress = 0;
            _options.MergeGridData = new MergeGridData();
            Save();
        }
    }

    public SaveOptions Options => _options;

    [ContextMenu("RemoveData")]
    public void RemoveData()
    {
        PlayerPrefs.DeleteKey(_dataKeyName);
        _options = new SaveOptions();
    }

    public void SetLevelIndex(int index)
    {
        _options.LevelNumber = index;
    }

    public void SetDateRegistration(DateTime date)
    {
        _options.RegistrationDate = date.ToString();
    }

    public void SetLastLoginDate(DateTime date)
    {
        _options.LastLoginDate = date.ToString();
    }

    public void SetCurrentSoft(int value)
    {
        _options.Soft = value;
    }

    public void AddSession()
    {
        _options.SessionCount++;
    }

    public void SetAbilityToHandleCarInAir()
    {
        _options.PlayerKnowsHowToHandleCarInFlight = true;
    }

    public string GetKeyName()
    {
        return _dataKeyName;
    }

    public int GetLevelIndex()
    {
        return _options.LevelNumber;
    }

    public int GetSessionCount()
    {
        return _options.SessionCount;
    }

    public void AddDisplayedLevelNumber()
    {
        _options.DisplayedLevelNumber++;
    }

    public int GetNumberDaysAfterRegistration()
    {
        return (DateTime.Parse(_options.LastLoginDate) - DateTime.Parse(_options.RegistrationDate)).Days;
    }

    public int GetDisplayedLevelNumber()
    {
        return _options.DisplayedLevelNumber;
    }

    public string GetRegistrationDate()
    {
        return _options.RegistrationDate;
    }

    public int GetCurrentSoft()
    {
        return _options.Soft;
    }

    public bool GetAbilityHandleCarInAir()
    {
        return _options.PlayerKnowsHowToHandleCarInFlight;
    }
}

[Serializable]
public class SaveOptions
{
    public int LevelNumber = 2;
    public int SessionCount;
    public string LastLoginDate;
    public string RegistrationDate;
    public int DisplayedLevelNumber = 1;
    public int Soft;
    public bool PlayerKnowsHowToHandleCarInFlight = false;
    public bool IsTutorial = true;

    public int KuzovUpgrade = 1;
    public int WheelsUpgrade = 1;
    public int EngineUpgrade = 1;
    public int PriceProgress = 0;

    public MergeGridData MergeGridData = new MergeGridData();
}

