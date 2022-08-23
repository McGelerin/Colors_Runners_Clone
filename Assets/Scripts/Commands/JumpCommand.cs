using Data.ValueObject;
using DG.Tweening;
using UnityEngine;

namespace Commands
{
    public class JumpCommand
    {
        #region Private Variables

        private PlayerData _playerData;
        private Transform _transform;
        
        #endregion
        public JumpCommand(ref PlayerData playerData,Transform transform)
        {
            _playerData = playerData;
            _transform = transform;
        }

        public void Execute()
        {
            _transform.DOMoveY(_playerData.MovementData.JumpDistance, _playerData.MovementData.JumpDuration)
                .SetEase(Ease.InOutCubic).SetAutoKill();
        }
    }
}