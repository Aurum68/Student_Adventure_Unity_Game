using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.Placement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AntonLed.StudentAdventure.UI
{
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
    {
        public Image itemIcon;

        private ItemData _item;

        public void SetItem(ItemData item)
        {
            _item = item;
            itemIcon.sprite = _item.icon;
            itemIcon.enabled = true;
        }

        public void Clear()
        {
            _item = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_item != null && _item.placementPrefab != null)
            {
                PlacementSystem.instance.StartPlacingInternal(_item);
            }
        }
    }
}
