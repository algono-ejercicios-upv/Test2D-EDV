using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class Movement : MonoBehaviour
    {
        public float speed = 10f;
        public float jumpForce = 30f;

        public bool freezeJump = false;

        private float RelativeSpeed => speed * Time.fixedDeltaTime;
        private float RelativeJumpForce => jumpForce * Time.fixedDeltaTime;

        private Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if (isActiveAndEnabled)
            {
                ApplyMovement();
                if (!freezeJump) ApplyJump();
            }
        }

        private void ApplyMovement()
        {
            Vector2 move = Vector2.zero;

            if (Input.GetKey(KeyCode.A))
            {
                move.x -= RelativeSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                move.x += RelativeSpeed;
            }

            rb.AddForce(move, ForceMode2D.Impulse);
        }

        private void ApplyJump()
        {
            Vector2 jump = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                jump.y += RelativeJumpForce;
            }

            if (Input.GetKey(KeyCode.S))
            {
                jump.y -= RelativeJumpForce;
            }

            rb.AddForce(jump, ForceMode2D.Impulse);
        }
    }
}
