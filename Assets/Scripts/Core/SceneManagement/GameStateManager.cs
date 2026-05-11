using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.Placement.Data;
using AntonLed.StudentAdventure.World;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntonLed.StudentAdventure.Core.SceneManagement
{
    public class GameStateManager: MonoBehaviour
    {
        public static GameStateManager instance;

        [SerializeField]
        public HashSet<string> collectedItemsIds = new HashSet<string>();

        [SerializeField]
        public Dictionary<string, List<PlacedItemData>> placedItems = new Dictionary<string, List<PlacedItemData>>();

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

        public void RegisterCollectedItem(string id)
        {
            if (!collectedItemsIds.Contains(id))
            {
                collectedItemsIds.Add(id);
            }

            string sceneName = SceneManager.GetActiveScene().name;

            if (placedItems.ContainsKey(sceneName))
            {
                List<PlacedItemData> itemsOnCurrentScene = placedItems[sceneName];
                PlacedItemData itemToRemove = itemsOnCurrentScene.FirstOrDefault(i => i.uniqueId == id);
                if (itemToRemove != null)
                {
                    itemsOnCurrentScene.Remove(itemToRemove);
                    Debug.Log($"<color=orange>Предмет с ID '{id}' подобран и удален из списка для сцены '{sceneName}'.</color>");
                }
            }
        }

        public void RegisterPlacedItem(PlacedItemData data)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (!placedItems.ContainsKey(sceneName))
            {
                placedItems[sceneName] = new List<PlacedItemData>();
            }

            List<PlacedItemData> itemsOnCurrentScene = placedItems[sceneName];
            PlacedItemData placedItemData = itemsOnCurrentScene.FirstOrDefault(i => i.uniqueId == data.uniqueId);
            if (placedItemData != null)
            {
                placedItemData.position = data.position;
                placedItemData.rotation = data.rotation;
            }
            else
            {
                itemsOnCurrentScene.Add(data);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"<color=yellow>СЦЕНА '{scene.name}' ЗАГРУЖЕНА. Начинаю расстановку предметов...</color>");

            if (!placedItems.ContainsKey(scene.name))
            {
                return;
            }

            List<PlacedItemData> itemsOnCurrentScene = placedItems[scene.name];

            foreach (PlacedItemData item in itemsOnCurrentScene)
            {
                ItemData itemData = item.itemData;
                GameObject prefab = itemData.placementPrefab;

                if (prefab != null)
                {
                    GameObject newObj = Instantiate(prefab, item.position, item.rotation);
                    WorldItem worldItem = newObj.GetComponent<WorldItem>();
                    if (worldItem != null)
                    {
                        worldItem.uniqueId = item.uniqueId;
                    }
                    Debug.Log($"<color=green>Предмет '{prefab.name}' воссоздан по координатам {item.position}</color>");
                }
            }
        }
    }
}