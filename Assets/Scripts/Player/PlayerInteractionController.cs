using AntonLed.StudentAdventure.Core.Interactable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntonLed.StudentAdventure.Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        public void OnClick(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            RaycastHit2D hit = GetHitAtMousePos();

            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        private RaycastHit2D GetHitAtMousePos()
        {
            int layerMask = LayerMask.GetMask("Interactable");

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            return Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        }
    }
}
