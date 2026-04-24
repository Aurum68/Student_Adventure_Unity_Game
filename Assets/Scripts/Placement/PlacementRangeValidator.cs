using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntonLed.StudentAdventure.Placement
{
    public class PlacementRangeValidator : MonoBehaviour
    {
        private HashSet<GameObject> _ghostsInRange = new HashSet<GameObject>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody != null)
            {
                GameObject rootObject = collision.attachedRigidbody.gameObject;
                _ghostsInRange.Add(rootObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.attachedRigidbody != null)
            {
                GameObject rootObject = collision.attachedRigidbody.gameObject;
                _ghostsInRange.Remove(rootObject);
            }
        }

        public bool IsGhostInRange(GameObject ghost)
        {
            if (ghost == null) return false;
            return _ghostsInRange.Contains(ghost);
        }
    }
}
