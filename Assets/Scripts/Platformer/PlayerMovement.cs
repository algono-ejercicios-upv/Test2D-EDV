using UnityEngine;


public class PlayerMovement : SimpleMovement
{
    public float speed = 10f, maxSpeed = 10f;
    public float jumpForce = 30f;

    public bool multiJump = false;

    public bool freezeJump = false, turnAround = true;

    private float RelativeSpeed => speed * Time.fixedDeltaTime;
    private float RelativeJumpForce => jumpForce * Time.fixedDeltaTime;

    private Animator anim;

    enum JumpState { grounded, jumping, floating, falling }

    private JumpState jumpState = JumpState.grounded;

    private Vector3 pScale = Vector3.one;

    private PlatformerManager gameManager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<PlatformerManager>();
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

    public void Freeze()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        anim.enabled = false;
        this.enabled = false;
    }

    public void Unfreeze()
    {
        rb.isKinematic = false;
        anim.enabled = true;
        this.enabled = true;
    }

    public void Respawn()
    {
        Respawn(gameManager.Checkpoint.transform);
    }

    private void Respawn(Transform checkpoint)
    {
        transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y, transform.position.z);
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

    public override void ApplyMovement()
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

    public override void ApplyJump()
    {
        if (freezeJump) return;

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

