using UnityEngine;

namespace Platformer
{
    public class Movement : MonoBehaviour
    {
        public float speed = 10f, maxSpeed = 10f;
        public float jumpForce = 30f;

        public bool multiJump = false;

        public bool freezeJump = false, turnAround = true;

        private float RelativeSpeed => speed * Time.fixedDeltaTime;
        private float RelativeJumpForce => jumpForce * Time.fixedDeltaTime;

        private Rigidbody2D rb;
        private Animator anim;

        enum JumpState { grounded, jumping, floating, falling }

        private JumpState jumpState = JumpState.grounded;

        private Vector3 pScale = Vector3.one;

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
            if (transform.InverseTransformPoint(collision.transform.position).y < 0)
            {
                SetJumpState(JumpState.grounded);

                MovingPlatform movingPlatform = collision.gameObject.GetComponent<MovingPlatform>();
                if (movingPlatform != null)
                {
                    transform.parent = collision.transform;
                    pScale = collision.transform.localScale;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (transform.parent != null && collision.gameObject == transform.parent.gameObject)
            {
                transform.parent = null;
                pScale = Vector3.one;
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
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            Physics2D.gravity = transform.up * -(Physics2D.gravity.magnitude);
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
            float deltaX = Input.GetAxis("Horizontal");
            float direction = Mathf.Sign(deltaX);

            if (!Mathf.Approximately(deltaX, 0))
            {
                Vector2 actualMove = transform.TransformVector(new Vector2(Mathf.Min(Mathf.Abs(deltaX) * RelativeSpeed, maxSpeed), 0f));

                rb.AddForce(actualMove, ForceMode2D.Impulse);
                anim.SetFloat("speed", RelativeSpeed);


                // If the player is upside down, look direction is inverted
                float lookDirection = Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, 180)) < 80f ? direction * -1 : direction;

                transform.localScale = new Vector3(lookDirection / pScale.x, 1 / pScale.y, 1 / pScale.z);
            }
            else
            {
                anim.SetFloat("speed", 0f);
            }
        }

        private void ApplyJump()
        {
            if (jumpState == JumpState.jumping)
            {
                if (rb.velocity.y == 0)
                    SetJumpState(JumpState.floating);
                else if (rb.velocity.y < 0)
                    SetJumpState(JumpState.falling);
            }

            if (Input.GetKey(KeyCode.Space) && (multiJump || jumpState == JumpState.grounded))
            {
                SetJumpState(JumpState.jumping);
                anim.SetBool("jumping", true);

                Jump(RelativeJumpForce);
            }
        }

        public void Jump(float force)
        {
            rb.AddForce(transform.TransformVector(Vector2.up * force), ForceMode2D.Impulse);
        }
    }
}
