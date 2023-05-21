using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlayClickSound);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.instance.UIClick.Play();
    }
}