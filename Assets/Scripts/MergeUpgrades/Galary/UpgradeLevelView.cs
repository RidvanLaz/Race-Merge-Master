using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLevelView : MonoBehaviour
{
    private const string LevelTextSample = "Level {0}";

    [SerializeField] private Image _openedImage;
    [SerializeField] private Image _closedImage;
    [SerializeField] private TMP_Text _levelText;

    public void Init(Sprite levelIcon, int levelIndex, bool isOpened)
    {
        _openedImage.sprite = levelIcon;
        _closedImage.sprite = levelIcon;

        _openedImage.gameObject.SetActive(isOpened);
        _closedImage.gameObject.SetActive(!isOpened);
        _levelText.text = String.Format(LevelTextSample, levelIndex);
    }
}