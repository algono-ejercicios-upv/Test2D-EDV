using System;
using UnityEngine;

public abstract class SimpleMovement : MonoBehaviour
{
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isActiveAndEnabled)
        {
            ApplyMovement();

            try
            {
                ApplyJump();
            }
            catch (NotImplementedException) { }
        }
    }

    public abstract void ApplyMovement();
    public virtual void ApplyJump()
    {
        throw new NotImplementedException();
    }
}
