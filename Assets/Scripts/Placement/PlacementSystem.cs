using AntonLed.StudentAdventure.Inventory;
using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.UI;
using AntonLed.StudentAdventure.World;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntonLed.StudentAdventure.Placement
{
    public class PlacementSystem : MonoBehaviour
    {
        public static PlacementSystem instance = null;

        private ItemData _itemData;
        private GameObject _mouseIndicator;
        private PlaceableItem _selectedItem;

        private Vector2 _mousePosition;
        private bool _isPlacing = false;

        [SerializeField]
        private string _placementGhostLayer = "PlacementGhost";

        [SerializeField]
        private PlacementRangeValidator _placementRangeValidator;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!_isPlacing)
            {
                return;
            }

            _mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(_mousePosition);
            _mouseIndicator.transform.position = worldPos;
        }

        public void StartPlacingInternal(ItemData item)
        {
            if (_isPlacing)
            {
                CancelPlacing();
            }

            InventoryUIManager.instance.ToggleInventory();

            _isPlacing = true;
            _itemData = item;

            _mouseIndicator = Instantiate(item.placementPrefab);
            _selectedItem = _mouseIndicator.GetComponent<PlaceableItem>();

            _selectedItem.itemData = _itemData;

            WorldItem worldItem = _mouseIndicator.GetComponent<WorldItem>();
            worldItem.uniqueId = System.Guid.NewGuid().ToString();

            _selectedItem.InitializeAsGhost();
            Utils.Utils.SetLayerRecursive(_mouseIndicator, _placementGhostLayer);
        }

        public void HandleClick(InputAction.CallbackContext context)
        {
            if (_isPlacing && context.performed)
            {
                if (_selectedItem.isValidLocation && _placementRangeValidator.IsGhostInRange(_mouseIndicator))
                {
                    PlaceItem();
                }
                else if (!_placementRangeValidator.IsGhostInRange(_mouseIndicator))
                {
                    Debug.Log("Предмет слишком далеко от игрока");
                }
            }
        }

        public void HandleCancel(InputAction.CallbackContext context)
        {
            if (_isPlacing && context.performed)
            {
                CancelPlacing();
            }
        }

        private void PlaceItem()
        {
            _selectedItem.Place();
            InventoryManager.instance.RemoveItem(_itemData);
            FindFirstObjectByType<InventoryUI>().UpdateUI();
            StopPlacingInternal();
        }

        private void CancelPlacing()
        {
            Destroy(_mouseIndicator);
            StopPlacingInternal();
        }

        private void StopPlacingInternal()
        { 
            _isPlacing = false;
            _mouseIndicator = null;
            _selectedItem = null;
            InventoryUIManager.instance.ToggleInventory();
        }

        public bool IsPlacing() { return _isPlacing; }
    }
}
