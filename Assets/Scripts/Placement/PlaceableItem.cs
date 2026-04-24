using AntonLed.StudentAdventure.Core.SceneMenegment;
using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.Placement.Data;
using AntonLed.StudentAdventure.Utils;
using AntonLed.StudentAdventure.World;
using UnityEditor.Build.Content;
using UnityEngine;

namespace AntonLed.StudentAdventure.Placement
{
    [RequireComponent (typeof(ItemLayerController), typeof(PlacementValidator), typeof(SpriteRenderer))]
    public class PlaceableItem : MonoBehaviour
    {
        private PlacementValidator _placementValidator;
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Color _validColor = Color.green;

        [SerializeField]
        private Color _invalidColor = Color.red;

        [SerializeField]
        private string _finalLayer = "Interactable";

        public bool isValidLocation;

        public ItemData itemData;
        private WorldItem _worldItem;

        private enum PlacementState
        {
            Placed, Placing
        }

        private PlacementState _currentState = PlacementState.Placed;

        private void Awake()
        {
            _placementValidator = GetComponent<PlacementValidator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _worldItem = GetComponent<WorldItem>();
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

            PlacedItemData placedItemData = new PlacedItemData(
                    itemData,
                    _worldItem.uniqueId,
                    transform.position,
                    transform.rotation
                );

            GameStateManager.instance.RegisterPlacedItem( placedItemData );

            Debug.Log($"Предмет '{itemData.name}' с ID '{_worldItem.uniqueId}' сохранен!");
        }

        public void InitializeAsGhost()
        {
            _placementValidator.enabled = true;
            this.enabled = true;
            _currentState = PlacementState.Placing;
        }
    }
}