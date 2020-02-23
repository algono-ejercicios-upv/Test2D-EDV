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
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Physics2D.gravity = Vector2.up * -(Physics2D.gravity.magnitude);
            }
        }
    }
}
