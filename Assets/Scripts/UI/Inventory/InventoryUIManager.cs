using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntonLed.StudentAdventure.UI
{
    public class InventoryUIManager : MonoBehaviour
    {
        public static InventoryUIManager instance;

        public GameObject inventoryPanel;

        private bool isInventoryOpen = false;

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

        private void Start()
        {
            inventoryPanel.SetActive(false);
        }

        public void ToggleInventory(InputAction.CallbackContext context)
        {
            ToggleInventory();
        }

        public void ToggleInventory()
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
        }
    }
}
