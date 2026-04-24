using AntonLed.StudentAdventure.Utils;
using UnityEngine;

namespace AntonLed.StudentAdventure.Placement
{
    [RequireComponent (typeof(ItemLayerController), typeof(PlacementValidator), typeof(SpriteRenderer))]
    public class PlaceableItem : MonoBehaviour
    {
        private ItemLayerController _layerController;
        private PlacementValidator _placementValidator;
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Color _validColor = Color.green;

        [SerializeField]
        private Color _invalidColor = Color.red;

        [SerializeField]
        private string _finalLayer = "Interactable";

        public bool isValidLocation;

        private enum PlacementState
        {
            Placed, Placing
        }

        private PlacementState _currentState = PlacementState.Placed;

        private void Awake()
        {
            _layerController = GetComponent<ItemLayerController>();
            _placementValidator = GetComponent<PlacementValidator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_currentState == PlacementState.Placing)
            {
                isValidLocation = _placementValidator.IsValidLocation;
                UpdateColorFeedback();
            }
        }

        private void UpdateColorFeedback()
        {
            _spriteRenderer.color = isValidLocation ? _validColor : _invalidColor;
        }

        public void Place()
        {
            _spriteRenderer.color = Color.white;

            _placementValidator.enabled = false;
            this.enabled = false;
            _currentState = PlacementState.Placed;
            Utils.Utils.SetLayerRecursive(gameObject, _finalLayer);
        }

        public void InitializeAsGhost()
        {
            _placementValidator.enabled = true;
            this.enabled = true;
            _currentState = PlacementState.Placing;
        }
    }
}