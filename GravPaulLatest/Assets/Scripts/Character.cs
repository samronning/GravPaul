using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    private float speed;

    //Surfaces
    protected LayerMask groundSurface;
    protected LayerMask spikeSurface;
    protected LayerMask wallSurface;

    protected bool isGrounded = false;

    protected Vector2 direction;
    protected Vector2 wallDirection;

    private Vector2 gravChangeDir;

    Animator anim;

    Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        groundSurface = LayerMask.NameToLayer("Ground");
        spikeSurface = LayerMask.NameToLayer("Spikes");
        wallSurface = (1 << groundSurface) | (1 << spikeSurface);
    }

    protected virtual void FixedUpdate()
    {
        Move();
        Flip();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Animate();
        Flip();
    }

    protected virtual void groundCheck()
    {
        Vector2 A = characterDrawOverlap()[0];
        Vector2 B = characterDrawOverlap()[1];
        Collider2D collision = (Physics2D.OverlapArea(A, B, (1 << groundSurface)));

        if (collision == null)
        {
            isGrounded = false;
            return;
        }
        else {
            isGrounded = true;
        }
    }

    protected virtual Vector2 wallCheck()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.55f, wallSurface);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.55f, wallSurface);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 0.55f, wallSurface);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.55f, wallSurface);

        if (hitRight)
        {
            direction = Vector2.zero;
            return Vector2.right;
        }
        if (hitLeft)
        {
            direction = Vector2.zero;
            return Vector2.left;
        }
        if (hitUp)
        {
            direction = Vector2.zero;
            return Vector2.up;
        }
        if (hitDown)
        {
            direction = Vector2.zero;
            return Vector2.down;
        }
        return Vector2.zero;
    }

    protected virtual Vector2[] characterDrawOverlap ()
    {
        //Setting up overlap draw
        Vector2 topLeft = new Vector2(transform.position.x - 0.20f, transform.position.y + 0.6f);
        Vector2 topRight = new Vector2(transform.position.x + 0.21f, transform.position.y + 0.6f);
        Vector2 bottomLeft = new Vector2(transform.position.x - 0.20f, transform.position.y - 0.6f);
        Vector2 bottomRight = new Vector2(transform.position.x + 0.21f, transform.position.y - 0.60f);

        Vector2 altTopLeft = new Vector2(transform.position.x - 0.6f, transform.position.y + 0.12f);
        Vector2 altTopRight = new Vector2(transform.position.x + 0.6f, transform.position.y + 0.20f);
        Vector2 altBottomLeft = new Vector2(transform.position.x - 0.61f, transform.position.y - 0.12f);
        Vector2 altBottomRight = new Vector2(transform.position.x + 0.61f, transform.position.y - 0.20f);

        //Right
        if (Physics2D.gravity.x > 0)
        {
            Vector2[] arrPoints = {altTopRight, altBottomRight};
            return arrPoints;
        }
        //Left
        if (Physics2D.gravity.x < 0)
        {
            Vector2[] arrPoints = { altTopLeft, altBottomLeft};
            return arrPoints;
        }
        //Up
        if (Physics2D.gravity.y > 0)
        {
            Vector2[] arrPoints = { topLeft, topRight};
            return arrPoints;
        }
        //Down
        if (Physics2D.gravity.y < 0)
        {
            Vector2[] arrPoints = { bottomLeft, bottomRight};
            return arrPoints;
        }
        else
        {
            return null;
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;

        if (direction.x != 0)
        {
            scale.x = direction.x;
            transform.localScale = scale;
        }
        if (Physics2D.gravity.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (Physics2D.gravity.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
        if (Physics2D.gravity.y > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        if (Physics2D.gravity.y < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Animate()
    {
        if (direction.x != 0 && rb.velocity.y == 0)
        {
            anim.SetBool("isWalking", true);
        }
        if (direction.x == 0)
        {
            anim.SetBool("isWalking", false);
        }
        if (Physics2D.gravity.y != 0)
        {
            if (rb.velocity.y != 0)
            {
                anim.SetBool("isFalling", true);
            }
            if (rb.velocity.y == 0)
            {
                anim.SetBool("isFalling", false);
            }
        }

        if (Physics2D.gravity.x != 0)
        {
            if (rb.velocity.x != 0)
            {
                anim.SetBool("isFalling", true);
            }
            if (rb.velocity.x == 0)
            {
                anim.SetBool("isFalling", false);
            }
        }

    }

    void OnDrawGizmos()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(center, Vector2.right * 0.6f);
        Gizmos.DrawRay(center, Vector2.left * 0.6f);
        Gizmos.DrawRay(center, (Vector2.right + (Vector2.down * 1.5f)) * 0.6f);
        Gizmos.DrawRay(center, (Vector2.left + (Vector2.down * 1.5f)) * 0.6f);
    }
}