using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class DontDestroyOnLoadHelper : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}