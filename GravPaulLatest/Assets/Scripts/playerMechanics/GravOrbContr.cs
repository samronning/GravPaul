using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravOrbContr : MonoBehaviour {
    Rigidbody2D rb;

    [SerializeField]
    private float orbSpeed = 10f;
    [SerializeField]
    private float decayTime = 2f;

    Animator gravOrbAnim;

    void Start () {
        gravOrbAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void LaunchGrav(GameObject gravBall, Vector2 direction)
    {
        rb = gravBall.GetComponent<Rigidbody2D>();
        rb.velocity = direction*orbSpeed;
        gravBall.transform.eulerAngles = new Vector3(0, 0, 0);
        Flip(gravBall);
        Destroy(gravBall, decayTime);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        gravOrbAnim.SetBool("isExploding", true);
        if (collision.gameObject.tag == "Grav")
        {
            cameraShake.shouldShake = true;
            foreach (ContactPoint2D hitPos in collision.contacts)
            {
                float gravCoef = Mathf.Abs(Physics2D.gravity.x + Physics2D.gravity.y);

                //Gravity right
                if (hitPos.relativeVelocity.x < 0)
                {
                    Physics2D.gravity = new Vector2(gravCoef, 0);
                    break;
                }
                //Gravity left
                if (hitPos.relativeVelocity.x > 0)
                {
                    Physics2D.gravity = new Vector2(-gravCoef, 0);
                    break;
                }
                //Gravity up
                if (hitPos.relativeVelocity.y < 0)
                {
                    Physics2D.gravity = new Vector2(0, gravCoef);
                    break;
                }
                //Gravity down
                if (hitPos.relativeVelocity.y > 0)
                {
                    Physics2D.gravity = new Vector2(0, -gravCoef);
                    break;
                }
            }
        }
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
        FindObjectOfType<audioManager>().Play("gravity_hit_wall");
        Destroy(gameObject, 0.3f);
    }

    void Flip(GameObject gravBall)
    {
        if (rb.velocity.x < 0)
          {
            gravBall.transform.eulerAngles += new Vector3(0, 0, 180);
          }
        if (rb.velocity.x > 0)
        {
            gravBall.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (rb.velocity.y < 0)
        {
            gravBall.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (rb.velocity.y > 0)
        {
            gravBall.transform.eulerAngles = new Vector3(0, 0, -90);
        }
    }
}
