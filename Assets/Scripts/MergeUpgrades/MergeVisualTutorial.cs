using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MergeGrid;
using UnityEngine;

public class MergeVisualTutorial : MonoBehaviour
{
    [SerializeField] private Transform _hand;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _clickTime;

    private MergeGridView _mergeGrid;
    private Upgrade _targetUpgrade;

    private Sequence _currentSequece;

    public void Init(MergeGridView mergeGrid)
    {
        _mergeGrid = mergeGrid;
        mergeGrid.Updated += UpdateAnimation;
    }

    public void SetTargetUpgrade(Upgrade upgrade)
    {
        _targetUpgrade = upgrade;
        UpdateAnimation();
    }

    public void Stop()
    {
        _currentSequece?.Kill();
        _mergeGrid.Updated -= UpdateAnimation;
        gameObject.SetActive(false);
    }

    private void UpdateAnimation()
    {
        _currentSequece?.Kill();

        if (_targetUpgrade != null)
        {
            _currentSequece = DOTween.Sequence();

            var possibleCells = _mergeGrid.GetCellForUpgrade(_targetUpgrade);

            if (possibleCells.Count >= 2)
            {
                var startPosition = possibleCells[1].transform.position;
                var endPosition = possibleCells[0].transform.position;

                _currentSequece.AppendCallback(() => _hand.position = startPosition);
                _currentSequece.Append(_hand.DOScale(0.9f, _clickTime));
                _currentSequece.Append(_hand.DOScale(1f, _clickTime));
                _currentSequece.Append(_hand.DOMove(endPosition, _moveTime));
                _currentSequece.Append(_hand.DOScale(0.9f, _clickTime));
                _currentSequece.Append(_hand.DOScale(1f, _clickTime));

                _currentSequece.SetLoops(-1);
            }
        }
    }

    private void OnDestroy()
    {
        _currentSequece?.Kill();
        _mergeGrid.Updated -= UpdateAnimation;
    }
}
