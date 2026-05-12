using AntonLed.StudentAdventure.Core.Audio;
using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _walkingSound;

        public void PlayWalkingSound()
        {
            AudioManager.instance.PlaySound(_walkingSound);
        }
    }
}