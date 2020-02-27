using System.Collections;
using UnityEngine;

namespace Platformer
{
    public class GravityShifter : MonoBehaviour
    {
        public float rotationSpeed = 100f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StopAllCoroutines();
                StartCoroutine(ApplyGravity(collision.transform));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StopAllCoroutines();
                StartCoroutine(ResetGravity(collision.transform));
            }
        }

        private IEnumerator ApplyGravity(Transform other)
        {
            yield return ApplyGravity(other, transform.up, Quaternion.LookRotation(Vector3.forward, transform.up));
        }

        private IEnumerator ResetGravity(Transform other)
        {
            yield return ApplyGravity(other, Vector2.up, Quaternion.identity);
        }

        private IEnumerator ApplyGravity(Transform other, Vector3 gravityDirection, Quaternion targetRotation)
        {
            Physics2D.gravity = gravityDirection * -(Physics2D.gravity.magnitude);

            other.transform.eulerAngles = new Vector3(0, 0, other.transform.eulerAngles.z); // Rotate only around z

            while (other.transform.rotation != targetRotation)
            {
                var step = rotationSpeed * Time.deltaTime;
                other.transform.rotation = Quaternion.RotateTowards(other.transform.rotation, targetRotation, step);

                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }
    }
}
