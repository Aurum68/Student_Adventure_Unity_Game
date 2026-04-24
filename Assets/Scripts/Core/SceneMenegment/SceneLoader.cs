using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Core.SceneMenegment
{
    public class SceneLoader : MonoBehaviour
    {
        public string sceneName;

        private void Start()
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}