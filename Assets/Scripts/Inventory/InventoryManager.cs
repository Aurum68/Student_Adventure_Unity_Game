using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.UI;
using System.Collections.Generic;
using UnityEngine;

namespace AntonLed.StudentAdventure.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager instance;

        public List<ItemData> items = new List<ItemData>();

        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;

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

        public void AddItem(ItemData item)
        {
            items.Add(item);

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }

            FindFirstObjectByType<InventoryUI>().UpdateUI();
        }

        public void RemoveItem(ItemData item)
        {
            items.Remove(item);

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }
}