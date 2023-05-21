using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MergeGrid
{
    public class MergeGridView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private List<CellView> _cells;
        [SerializeField] private MergeGridElementView _mergeGridElementPrefab;
        [SerializeField] private RectTransform _mergeGridElementsParent;
        [SerializeField] private RectTransform _buyingZone;
        [SerializeField] private SellingZone _sellingZone;
        [SerializeField] private GameObject _mergeFX;

        private List<MergeGridElementView> _currentViews = new List<MergeGridElementView>();
        private Upgrade[] _upgrades;
        private MergeGridElementView _currentlyDragging;
        private CellView _draggedFrom;
        private Vector3 _dragPosition;
        private Money _money;

        public event Action Updated;

        public bool HasEmptyCells
        {
            get
            {
                var cell = _cells.Find(x => x.HasContent == false);
                return cell != null;
            }
        }

        public void Init(Upgrade[] upgrades, Money money)
        {
            _money = money;
            _upgrades = upgrades;
            _sellingZone.Init(_money);

            foreach (var upgrade in upgrades)
            {
                CreateElement(upgrade, 1, _cells[_currentViews.Count]);
            }
            UpdateUpgrades();
        }

        public void AddElement(Upgrade upgrade)
        {
            CreateElement(upgrade, 1, GetFirstEmptyCell());
            Updated?.Invoke();
            UpdateUpgrades();
        }

        private int GetElementsCountWithUpgrade(Upgrade upgrade)
        {
            var cells = _cells.FindAll(x => x.HasContent && x.Content.Data.TargetUpgrade == upgrade);
            return cells.Count;
        }

        private void CreateElement(Upgrade upgrade, int level, CellView cell)
        {
            var mergeElement = new MergeGridElement(level, upgrade);
            var newView = Instantiate(_mergeGridElementPrefab, _mergeGridElementsParent);
            newView.Init(mergeElement);

            cell.TryPlace(newView, true);
            _currentViews.Add(newView);
        }

        private CellView GetFirstEmptyCell()
        {
            var cell = _cells.Find(x => x.HasContent == false);
            if (cell != null)
                return cell;
            else
                throw new Exception("Grid is full!");
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_currentlyDragging != null)
                return;

            var clickCell = GetClosestCell(eventData.position);

            if (clickCell.HasContent)
            {
                _dragPosition = eventData.position;
                _draggedFrom = clickCell;
                _currentlyDragging = clickCell.TakeContent();
                _currentlyDragging.transform.SetAsLastSibling();

                foreach (var cellView in _cells)
                {
                    cellView.SetDark(clickCell != cellView);
                }

                if (GetElementsCountWithUpgrade(_currentlyDragging.Data.TargetUpgrade) > 0)
                {
                    _buyingZone.gameObject.SetActive(false);
                    _sellingZone.Open(_currentlyDragging.Data);
                }

                SoundManager.instance.UIClick.Play();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_currentlyDragging == null)
                return;
            
            if (GetElementsCountWithUpgrade(_currentlyDragging.Data.TargetUpgrade) > 0 && _sellingZone.IsInZone(eventData.position))
            {
                SoundManager.instance.UIClick.Play();
                _sellingZone.Sell(_currentlyDragging.Data);
                _currentViews.Remove(_currentlyDragging);
                Destroy(_currentlyDragging.gameObject);
            }
            else
            {
                var cell = GetClosestCell(eventData.position);
                var placeResult = cell.TryPlace(_currentlyDragging);

                switch (placeResult.result)
                {
                    case PlacingResult.Place:
                        SoundManager.instance.UIClick.Play();
                        break;


                    case PlacingResult.Merge:
                        _currentViews.Remove(_currentlyDragging);
                        SoundManager.instance.Merge.Play();
                        var fx = Instantiate(_mergeFX, transform);
                        fx.transform.position = cell.transform.position;
                        Destroy(fx, 1f);
                        break;


                    case PlacingResult.Replace:
                        _draggedFrom.TryPlace(placeResult.element);
                        SoundManager.instance.UIClick.Play();
                        break;
                }
            }

            foreach (var cellView in _cells)
            {
                cellView.SetDark(false);
            }

            _buyingZone.gameObject.SetActive(true);
            _draggedFrom = null;
            _currentlyDragging = null;
            _sellingZone.gameObject.SetActive(false);
            Updated?.Invoke();

            UpdateUpgrades();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_currentlyDragging == null)
                return;

            _dragPosition = eventData.position;
        }

        private void UpdateUpgrades()
        {
            var biggestCells = new List<CellView>();
            foreach (var upgrade in _upgrades)
            {
                CellView maxLevelCell = null;
                foreach (var cellView in _cells)
                {
                    if (cellView.HasContent && cellView.Content.Data.TargetUpgrade == upgrade)
                    {
                        if (maxLevelCell == null || maxLevelCell.Content.Data.Level < cellView.Content.Data.Level)
                            maxLevelCell = cellView;
                    }
                }

                upgrade.SetLevel(maxLevelCell.Content.Data.Level);
                biggestCells.Add(maxLevelCell);
            }

            foreach (var cellView in _cells)
            {
                cellView.MarkAsMain(biggestCells.Contains(cellView));
            }
        }

        private CellView GetClosestCell(Vector2 position)
        {
            CellView closestCell = null;
            float closestDistance = float.MaxValue;

            foreach (var cellView in _cells)
            {
                var currentDistance = Vector2.Distance(position, cellView.transform.position);
                if (closestCell == null || currentDistance < closestDistance)
                {
                    closestCell = cellView;
                    closestDistance = currentDistance;
                }
            }

            return closestCell;
        }

        [EditorButton]
        private void AddCar()
        {
            AddElement(_upgrades[0]);
        }

        [EditorButton]
        private void AddEngine()
        {
            AddElement(_upgrades[1]);
        }

        [EditorButton]
        private void AddWheel()
        {
            AddElement(_upgrades[2]);
        }

        private void LateUpdate()
        {
            if (_currentlyDragging != null)
                _currentlyDragging.transform.position = Vector3.Lerp(_currentlyDragging.transform.position, _dragPosition, 6f * Time.deltaTime);
        }
    }

    public enum PlacingResult
    {
        Place,
        Replace,
        Merge
    }
}
