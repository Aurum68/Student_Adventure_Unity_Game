using UnityEngine;


namespace AntonLed.StudentAdventure.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null )
            {
                instance = this;
                DontDestroyOnLoad(gameObject.transform.root.gameObject);
            }
            else {Destroy(gameObject);}
        }

        public void PlaySound(AudioClip clip)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}

