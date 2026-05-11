using AntonLed.StudentAdventure.Inventory;
using AntonLed.StudentAdventure.Inventory.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AntonLed.StudentAdventure.UI
{
    public class InventoryUI : MonoBehaviour
    {
        //public Transform itemsParent;

        [SerializeField]
        private List<InventorySlotUI> _itemSlots;

        public Button nextPage;
        public Button previousPage;

        private int _currentPage = 0;

        private int _slotsPerPage;

        private void Awake()
        {
            _slotsPerPage = _itemSlots.Count;
        }

        private void OnEnable()
        {
            _currentPage = 0;
            UpdateUI();
        }

        public void UpdateUI()
        {
            DrawCurrentPage();
            UpdateNavigationButtons();
        }

        private void DrawCurrentPage()
        {
            int startIndex = _currentPage * _slotsPerPage;

            List<ItemData> items = InventoryManager.instance.items;
            int itemsCout = items.Count;

            for (int i = 0; i < _itemSlots.Count; i++)
            {
                int itemIndex = startIndex + i;

                if (itemIndex < itemsCout)
                {
                    _itemSlots[i].SetItem(items[itemIndex]);
                }
                else
                {
                    _itemSlots[i].Clear();
                }
            }
        }

        private void UpdateNavigationButtons()
        {
            previousPage.interactable = (_currentPage >  0);

            int totalItems = InventoryManager.instance.items.Count;
            int nextPageIndex = (_currentPage + 1) * _slotsPerPage;
            nextPage.interactable = (nextPageIndex <  totalItems);
        }

        public void NextPage()
        {
            _currentPage++;
            UpdateUI();
        }

        public void PreviousPage()
        {
            _currentPage--;
            UpdateUI();
        }
    }
}
