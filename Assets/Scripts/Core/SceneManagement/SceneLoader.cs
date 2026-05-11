using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntonLed.StudentAdventure.Core.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private string _sceneToLoad = "SampleScene";

        void Start()
        {
            if (Core.SceneManagement.SceneManagement.instance != null)
            {
                Core.SceneManagement.SceneManagement.instance.LoadScene(_sceneToLoad);
            }
            else
            {
                Debug.LogError("SceneManagement.instance не найден! Не могу загрузить начальную сцену.");
            }
        }
    }
}