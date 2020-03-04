using UnityEngine;

namespace Watermelon
{
    public class Movement : SimpleMovement
    {
        public float speed = 10f;
        public float jumpForce = 30f;

        public bool freezeJump = false;

        private float RelativeSpeed => speed * Time.fixedDeltaTime;
        private float RelativeJumpForce => jumpForce * Time.fixedDeltaTime;

        public override void ApplyMovement()
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

        public override void ApplyJump()
        {
            if (freezeJump) return;

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
