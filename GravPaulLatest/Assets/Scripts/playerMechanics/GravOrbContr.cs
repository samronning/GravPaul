using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravOrbContr : MonoBehaviour {
    Rigidbody2D rb;

    [SerializeField]
    private float orbSpeed = 10f;
    [SerializeField]
    private float decayTime = 2f;

    public void LaunchGrav(GameObject gravBall, Vector2 direction)
    {
        rb = gravBall.GetComponent<Rigidbody2D>();
        rb.velocity = direction*orbSpeed;
        Destroy(gravBall, decayTime);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
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
        Destroy(gameObject);
    }
}
