using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MergeGrid
{
    public class MergeGridElementView : MonoBehaviour
    {
        [Space]
        [SerializeField] private Image _contentImage;
        [SerializeField] private TMP_Text _levelText;

        private MergeGridElement _data;
        private Tween _currentTween;
        private bool _isPressed;

        public MergeGridElement Data => _data;

        public void Init(MergeGridElement data)
        {
            _data = data;
            UpdateView();
        }

        public void Upgrade()
        {
            _data.Upgrade();
            UpdateView();
            //show effect
        }

        public void MoveToPosition(Vector2 position)
        {
            _currentTween?.Kill();
            _currentTween = transform.DOMove(position, 0.25f);
        }

        public bool IsEqual(MergeGridElementView other)
        {
            return Data.TargetUpgrade == other.Data.TargetUpgrade && Data.Level == other.Data.Level;
        }

        private void UpdateView()
        {
            _contentImage.sprite = _data.CurrentIcon;
            _levelText.text = _data.Level.ToString();
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }
    }
}