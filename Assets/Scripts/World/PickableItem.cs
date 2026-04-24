using AntonLed.StudentAdventure.Core.Interactable;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isPlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
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
