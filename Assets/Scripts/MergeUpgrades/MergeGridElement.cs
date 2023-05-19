using System;
using UnityEngine;

namespace MergeGrid
{
    public class MergeGridElement
    {
        private int _level;
        private Upgrade _targetUpgrade;

        public MergeGridElement(int level, Upgrade targetUpgrade)
        {
            _level = level;
            _targetUpgrade = targetUpgrade;
        }

        public Upgrade TargetUpgrade => _targetUpgrade;
        public int Level => _level;

        public Sprite CurrentIcon => _targetUpgrade.GetSpriteForLevel(Level);

        public void Upgrade()
        {
            _level = (int)MathF.Min(_level + 1, _targetUpgrade.MaxLevel);
        }
    }
}