using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntonLed.StudentAdventure.Core.SceneManagement
{
    public class SceneManagement : MonoBehaviour
    {
        public static SceneManagement instance;

        public static event System.Action onSceneLoadStart;
        public static event System.Action onSceneLoadFinish;

        [Header("Configuration")]
        [SerializeField]
        private CanvasGroup _loadingCanvasGroup;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject.transform.root.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            onSceneLoadStart?.Invoke();

            yield return StartCoroutine(FadeLoadingScreen(1f, _fadeDuration));

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);

            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            yield return StartCoroutine(FadeLoadingScreen(0f, _fadeDuration));

            onSceneLoadFinish?.Invoke();
        }

        private IEnumerator FadeLoadingScreen(float targetAlpha, float duration)
        {
            if (targetAlpha > 0)
            {
                _loadingCanvasGroup.interactable = true;
                _loadingCanvasGroup.blocksRaycasts = true;
            }

            float startAlpha = _loadingCanvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                _loadingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                yield return null;
            }

            _loadingCanvasGroup.alpha = targetAlpha;

            if (targetAlpha == 0)
            {
                _loadingCanvasGroup.interactable = false;
                _loadingCanvasGroup.blocksRaycasts = false;
            }
        }
    }
}