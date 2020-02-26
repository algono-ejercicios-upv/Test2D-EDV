using UnityEngine;

namespace Platformer
{
    public class GravityShifter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Physics2D.gravity = transform.up * -(Physics2D.gravity.magnitude);
                collision.transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.up);

                Movement playerMovement = collision.GetComponent<Movement>();
                if (playerMovement.lookInverted) playerMovement.TurnAround(false);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Physics2D.gravity = Vector2.up * -(Physics2D.gravity.magnitude);
                
                Movement playerMovement = collision.GetComponent<Movement>();
                playerMovement.ResetRotation();

            }
        }
    }
}
