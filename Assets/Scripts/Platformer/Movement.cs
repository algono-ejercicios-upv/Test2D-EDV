using UnityEngine;

namespace Platformer
{
    public class Movement : MonoBehaviour
    {
        public float speed = 10f;
        public float jumpForce = 30f;

        public bool multiJump = false;

        public bool freezeJump = false, turnAround = true, stickToGround = true;

        private float RelativeSpeed => speed * Time.fixedDeltaTime;
        private float RelativeJumpForce => jumpForce * Time.fixedDeltaTime;

        private Rigidbody2D rb;
        private Animator anim;

        enum JumpState { grounded, jumping, floating, falling }

        private JumpState jumpState = JumpState.grounded;

        private bool lookInverted = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            if (isActiveAndEnabled)
            {
                ApplyMovement();

                if (!freezeJump) ApplyJump();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((collision.transform.position - this.transform.position).y < 0)
            {
                SetJumpState(JumpState.grounded);
                
                if (stickToGround && collision.collider.CompareTag("Ground"))
                {
                    var newUp = collision.collider.transform.up;
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, newUp);
                    if (lookInverted) TurnAround(false);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Respawn"))
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            transform.position = new Vector3(0f, 0f, transform.position.z);
            rb.velocity = Vector2.zero;
            lookInverted = false;
            ResetRotation();
            Physics2D.gravity = transform.up * -(Physics2D.gravity.magnitude);
        }

        private void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
            if (lookInverted) TurnAround(false);
        }

        private void SetJumpState(JumpState value)
        {
            jumpState = value;

            if (value == JumpState.grounded)
            {
                anim.SetBool("jumping", false);
            }
        }

        private void ApplyMovement()
        {
            bool move = false;

            if (Input.GetKey(KeyCode.A))
            {
                move = true;
                if (!lookInverted) TurnAround();
            }

            if (Input.GetKey(KeyCode.D))
            {
                move = true;
                if (lookInverted) TurnAround();
            }

            if (move)
            {
                Vector2 actualMove = transform.TransformVector(new Vector2(RelativeSpeed, 0f));
                rb.AddForce(actualMove, ForceMode2D.Impulse);
                anim.SetFloat("speed", RelativeSpeed);
            }
            else
            {
                anim.SetFloat("speed", 0f);
            } 
        }

        private void TurnAround(bool updateLook = true)
        {
            transform.Rotate(Vector2.up, 180f);
            if (updateLook) lookInverted = !lookInverted;
        }

        private void ApplyJump()
        {
            Vector2 jump = Vector2.zero;

            if (jumpState == JumpState.jumping)
            {
                if (rb.velocity.y == 0)
                    SetJumpState(JumpState.floating);
                else if (rb.velocity.y < 0)
                    SetJumpState(JumpState.falling);
            }

            if (Input.GetKey(KeyCode.Space) && (multiJump || jumpState == JumpState.grounded))
            {
                jump.y += RelativeJumpForce;
                SetJumpState(JumpState.jumping);
                anim.SetBool("jumping", true);
            }

            rb.AddForce(transform.TransformVector(jump), ForceMode2D.Impulse);
        }
    }
}
