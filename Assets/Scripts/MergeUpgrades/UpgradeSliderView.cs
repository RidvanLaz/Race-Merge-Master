using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSliderView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _name;

    private Upgrade _upgrade;

    public void Init(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _slider.normalizedValue = _upgrade.Progress;
        _name.text = upgrade.Name;

        _upgrade.LevelChanged += UpdateSlider;
    }

    private void UpdateSlider(int newLevel)
    {
        _slider.normalizedValue = _upgrade.Progress;
    }
}