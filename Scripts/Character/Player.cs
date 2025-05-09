using System;
using UnityEngine;

namespace Character
{
    public class Player: Character
    {
        private Touch _playerTouch;
        private Vector2 _touchStartingPosition;


        protected override void CalculateMovementVector()
        {
            if (Input.touchCount <= 0){ return;}
            
            _playerTouch = Input.GetTouch(0);
            switch (_playerTouch.phase)
            {
                case TouchPhase.Began:
                    _touchStartingPosition = _playerTouch.position;
                    break;
                case TouchPhase.Moved:
                    MovementVector = _playerTouch.position - _touchStartingPosition;
                    break;
                case TouchPhase.Stationary:
                    MovementVector = _playerTouch.position - _touchStartingPosition;
                    break;
                case TouchPhase.Ended:
                    MovementVector = Vector2.zero;
                    break;
                case TouchPhase.Canceled:
                    MovementVector = Vector2.zero;
                    break;
                default:
                    MovementVector = Vector2.zero;
                    break;
                    throw new ArgumentOutOfRangeException(); // :-)
            }
        }
    }
}