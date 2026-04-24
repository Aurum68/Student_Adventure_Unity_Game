using AntonLed.StudentAdventure.Core.Interactable;
using AntonLed.StudentAdventure.Core.SceneMenegment;
using AntonLed.StudentAdventure.Inventory;
using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.Placement;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace AntonLed.StudentAdventure.World
{
    public class PickableItem : MonoBehaviour, IInteractable
    {
        public ItemData ItemData;

        private bool _isPlayerInRange = false;
        private WorldItem _worldIem;

        private void Awake()
        {
            _worldIem = GetComponent<WorldItem>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerInteractArea"))
            {
                Debug.Log("Player in range");
                _isPlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerInteractArea"))
            {
                Debug.Log("Player out of range");
                _isPlayerInRange = false;
            }
        }

        public void Interact()
        {
            if (PlacementSystem.instance.IsPlacing())
            {
                return;
            }

            if (_isPlayerInRange)
            {
                GameStateManager.instance.RegisterCollectedItem(_worldIem.uniqueId);
                InventoryManager.instance.AddItem(ItemData);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Далековато!");
            }
        }
    }
}
