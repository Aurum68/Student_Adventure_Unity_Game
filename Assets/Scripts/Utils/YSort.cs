using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class YSort : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            _spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
        }
    }
}
