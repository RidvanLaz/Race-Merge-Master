using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeGrid
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _mainFrame;
        [SerializeField] private Image _back;

        [Space] 
        [SerializeField] private Color _standartColor;
        [SerializeField] private Color _darkColor;
        [SerializeField] private Color _selectedColor;

        private MergeGridElementView _content;
        private bool _isMarkedAsMain;

        public bool HasContent => _content != null;
        public bool IsMarkedAsMain => _isMarkedAsMain;

        public MergeGridElementView Content => _content;

        public (MergeGridElementView element, PlacingResult result) TryPlace(MergeGridElementView content, bool isInstant = false)
        {
            if (_content == null)
            {
                _content = content;

                if (isInstant)
                    _content.transform.position = transform.position;
                else
                    _content.MoveToPosition(transform.position);

                return (content, PlacingResult.Place);
            }
            else
            {
                if (_content.IsEqual(content))
                {
                    _content.Upgrade();
                    Destroy(content.gameObject);
                    return (_content, PlacingResult.Merge);
                }
                else
                {
                    var oldContent = _content;
                    _content = content;

                    if (isInstant)
                        _content.transform.position = transform.position;
                    else
                        _content.MoveToPosition(transform.position);

                    return (oldContent, PlacingResult.Replace);
                }
            }
        }

        public MergeGridElementView TakeContent()
        {
            var content = _content;
            _content = null;
            return content;
        }

        public void SetDark(bool isDark)
        {
            _back.color = isDark ? _darkColor : _standartColor;
        }

        public void MarkAsMain(bool isMain)
        {
            _isMarkedAsMain = isMain;
            _back.color = isMain ? _selectedColor : _standartColor;
        }
    }
}