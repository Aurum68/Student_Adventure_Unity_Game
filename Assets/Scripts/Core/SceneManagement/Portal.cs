using AntonLed.StudentAdventure.Core.Interactable;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntonLed.StudentAdventure.Core.SceneManagement
{
    internal class Portal : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private string _sceneName;

        [SerializeField]
        private SpawnPointId _targetSpawnPointId;

        public void Interact()
        {
            SceneTransitionData.targetSpawnPointId = _targetSpawnPointId;
            //SceneManager.LoadScene(_sceneName);
            SceneManagement.instance.LoadScene(_sceneName);
        }
    }
}