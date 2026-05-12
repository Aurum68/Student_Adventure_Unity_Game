using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.Core.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        private AudioClip currentMusic;
        private AudioSource audioSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject.transform.root.gameObject);
            }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            currentMusic = gameObject.GetComponent<AudioSource>().clip;
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void SetMusic(AudioClip clip)
        {
            if (clip == null || clip == currentMusic)
            {
                return;
            }

            currentMusic = clip;

            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}