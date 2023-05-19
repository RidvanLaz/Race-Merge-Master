using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private Money _money;

    public void Init(Money money)
    {
        _money = money;
        _money.Updated += UpdateText;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = _money.CurrentMoney.ToString();
    }

    private void OnDestroy()
    {
        _money.Updated -= UpdateText;
    }
}