using UnityEngine;

public class RpgPlayerMovement : SimpleMovement
{
    public float speed = 100f;

    public float RelativeSpeed => speed * Time.fixedDeltaTime;

    private Animator anim;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void ApplyMovement()
    {
        Vector2 movement = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };

        movement.Normalize();

        float direction = Mathf.Sign(movement.x);
        movement.x = Mathf.Abs(movement.x);

        movement *= RelativeSpeed;
        Vector2 actualMove = transform.TransformVector(movement);

        rb.velocity = actualMove;
        anim.SetFloat("speedX", movement.x);
        anim.SetFloat("speedY", movement.y);

        if (!Mathf.Approximately(movement.magnitude, 0))
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }
}
