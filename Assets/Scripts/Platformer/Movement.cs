using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Movement : MonoBehaviour
    {
        public float speed = 10f;
        public float jumpForce = 30f;

        public bool freezeJump = false, freezeRotation = false, avoidSliding = true;

        private float RelativeSpeed => speed * Time.fixedDeltaTime;
        private float RelativeJumpForce => jumpForce * Time.fixedDeltaTime;

        private Rigidbody2D rb;
        private Animator anim;

        enum JumpState { grounded, jumping, floating, falling }

        private JumpState jumpState = JumpState.grounded;

        private bool lookInverted = false;

        private float originalGravityScale;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        //void Update()
        //{

        //}

        void FixedUpdate()
        {
            if (isActiveAndEnabled)
            {
                Vector2 moved = ApplyMovement();

                if (avoidSliding) CheckGravity(moved);

                if (!freezeRotation) CheckRotation();
                if (!freezeJump) ApplyJump();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (jumpState != JumpState.grounded
                && (collision.transform.position - this.transform.position).y < 0)
            {
                jumpState = JumpState.grounded;
                anim.SetBool("jumping", false);
            }
        }

        // Avoid player sliding on diagonal platforms
        private void CheckGravity(Vector2 moved)
        {
            if (jumpState == JumpState.grounded
                && moved.magnitude == 0
                && Mathf.Approximately(rb.velocity.magnitude, 0f))
            {
                if (rb.gravityScale > 0)
                    originalGravityScale = rb.gravityScale;

                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = originalGravityScale;
            }
        }

        private void CheckRotation()
        {
            float xVelocity = rb.velocity.x;

            if (xVelocity < 0 && !lookInverted)
            {
                transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
                lookInverted = !lookInverted;
            }
            else if (xVelocity > 0 && lookInverted)
            {
                transform.rotation = Quaternion.identity;
                lookInverted = !lookInverted;
            }
        }


        private Vector2 ApplyMovement()
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

            anim.SetFloat("speed", move.magnitude);

            rb.AddForce(move, ForceMode2D.Impulse);

            return move;
        }

        private void ApplyJump()
        {
            Vector2 jump = Vector2.zero;

            if (jumpState == JumpState.jumping)
            {
                if (rb.velocity.y == 0)
                    jumpState = JumpState.floating;
                else if (rb.velocity.y < 0)
                    jumpState = JumpState.falling;
            }

            if (Input.GetKey(KeyCode.Space) && jumpState == JumpState.grounded)
            {
                jump.y += RelativeJumpForce;
                jumpState = JumpState.jumping;
                anim.SetBool("jumping", true);
            }

            //if (Input.GetKey(KeyCode.S))
            //{
            //    jump.y -= RelativeJumpForce;
            //}

            rb.AddForce(jump, ForceMode2D.Impulse);
        }
    }
}
