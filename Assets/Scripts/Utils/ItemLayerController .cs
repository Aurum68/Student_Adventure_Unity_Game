using System.Linq;
using UnityEngine;

namespace AntonLed.StudentAdventure.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemLayerController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private LayerMask layerMask;
    
        public bool isPlaced = false;

        private int _surfacesTouchingCount = 0;

        private void Awake()
        {
            int interactiveLayer = LayerMask.NameToLayer("Interactable");
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (!isPlaced && (layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                _surfacesTouchingCount++;
                UpdateState();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isPlaced && (layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                _surfacesTouchingCount--;
                UpdateState();
            }
        }

        private void UpdateState()
        {
            isPlaced = _surfacesTouchingCount > 0;

            if (isPlaced)
            {
                SetOnSurface();
            }
            else
            {
                SetOnFloor();
            }
        }

        private void SetOnFloor()
        {
            _spriteRenderer.sortingLayerName = "Player";
        }

        private void SetOnSurface()
        {
            _spriteRenderer.sortingLayerName = "OnSurfaceItem";
        }
    }
}
