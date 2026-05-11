using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.Utils
{
    public class Spinner : MonoBehaviour
    {

        [SerializeField]
        private float _speed = 200f;

        void Update()
        {
            transform.Rotate(0, 0, -_speed * Time.deltaTime);
        }
    }
}