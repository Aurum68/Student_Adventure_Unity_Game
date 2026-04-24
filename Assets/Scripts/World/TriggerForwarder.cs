using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.World
{
    public class TriggerForwarder : MonoBehaviour
    {
        [SerializeField]
        private Stairs mainController;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                mainController.OnPlayerEnterTrigger(this.gameObject, collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                mainController.OnPlayerExitTrigger(this.gameObject, collision);
            }
        }
    }
}
