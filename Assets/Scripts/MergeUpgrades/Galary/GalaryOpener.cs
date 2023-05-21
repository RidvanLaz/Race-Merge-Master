using DG.Tweening;
using UnityEngine;

public class GalaryOpener : MonoBehaviour
{
    [SerializeField] private RectTransform _movableObject;
    [SerializeField] private float _mergeGridX;
    [SerializeField] private float _galaryX;

    public void OpenGridPanel()
    {
        _movableObject.DOAnchorPosX(_mergeGridX, 0.3f);
    }

    public void OpenGalaryPanel()
    {
        _movableObject.DOAnchorPosX(_galaryX, 0.3f);
    }
}