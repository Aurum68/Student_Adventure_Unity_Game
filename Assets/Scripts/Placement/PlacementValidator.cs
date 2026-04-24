using System.Collections;
using System.Linq;
using UnityEngine;

namespace AntonLed.StudentAdventure.Placement
{
    public class PlacementValidator : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;

        private int _invalidTouchingCount = 0;

        public bool IsValidLocation { get; private set; } = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                _invalidTouchingCount++;
                UpdateValidationStatus();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                _invalidTouchingCount--;
                UpdateValidationStatus();
            }
        }

        private void UpdateValidationStatus()
        {
            IsValidLocation = _invalidTouchingCount <= 0;
        }
    }
}
