using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace AntonLed.StudentAdventure.World
{
    public class Stairs : MonoBehaviour
    {
        [SerializeField]
        private Collider2D[] _railingsColliders;

        [SerializeField]
        private GameObject _entryTriggerDown;

        [SerializeField] 
        private GameObject _entryTriggerUp;

        [SerializeField]
        private Collider2D _entryCollider;

        [SerializeField]
        private Collider2D _wallCollider;

        [SerializeField]
        private GameObject _areaTrigger;

        private SpriteRenderer _playerRenderer;

        private HashSet<GameObject> _activeTriggers = new HashSet<GameObject>();

        private const string PLAYER_LAYER = "Player";
        private const string BEHIND_LAYER = "InteriorCollision";

        private enum PlayerStairState
        {
            Outside, Stairs, Under
        }

        private PlayerStairState _state = PlayerStairState.Outside;

        public void OnPlayerEnterTrigger(GameObject trigger, Collider2D playerCollider)
        {
            Debug.Log($"<color=cyan><b>--- OnPlayerEnterTrigger ---</b></color>\n" +
                 $"Триггер: <b>{trigger.name}</b>\n" +
                 $"Текущее состояние: <b>{_state}</b>");

            if (_playerRenderer == null)
            {
                _playerRenderer = playerCollider.GetComponentInChildren<SpriteRenderer>();
            }

            if (_playerRenderer == null)
            {
                return;
            }

            _activeTriggers.Add( trigger );

            if ((trigger == _entryTriggerDown || trigger == _entryTriggerUp) && _state == PlayerStairState.Outside)
            {
                Debug.Log("<color=green>РЕШЕНИЕ: Игрок входит на лестницу. Меняю состояние на Stairs.</color>");

                _state = PlayerStairState.Stairs;
                _entryCollider.enabled = false;
                _wallCollider.enabled = false;
                foreach (var collider in _railingsColliders)
                {
                    collider.enabled = true;
                }
            }
            else if (trigger == _areaTrigger && _state == PlayerStairState.Outside)
            {
                Debug.Log("<color=yellow>РЕШЕНИЕ: Игрок входит под лестницу. Меняю состояние на Under.</color>");

                _state = PlayerStairState.Under;
                _playerRenderer.sortingLayerName = BEHIND_LAYER;
                foreach (var collider in _railingsColliders)
                {
                    collider.enabled = false;
                }
                _entryCollider.enabled = true;
                _wallCollider.enabled = true;
            }
        }

        public void OnPlayerExitTrigger(GameObject trigger, Collider2D playerCollider)
        {
            Debug.Log($"<color=orange><b>--- OnPlayerExitTrigger ---</b></color>\n" +
                  $"Триггер: <b>{trigger.name}</b>\n" +
                  $"Текущее состояние: <b>{_state}</b>");

            if (_playerRenderer == null) return;

            _activeTriggers.Remove( trigger );

            if (_activeTriggers.Count == 0 )
            {
                Debug.Log("<color=green>РЕШЕНИЕ: Игрок покидает активную зону. Сбрасываю состояние на Outside.</color>");

                _state = PlayerStairState.Outside;
                _playerRenderer.sortingLayerName = PLAYER_LAYER;
                _entryCollider.enabled = false;
                _wallCollider.enabled = false;

                foreach (var collider in _railingsColliders)
                {
                    collider.enabled = true;
                }
            }
        }
    }

}
