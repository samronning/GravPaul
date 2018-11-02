using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveablePlatform : MonoBehaviour {

    float maxSpeed = 20f;
    public Vector2 startDirection;
    public float distance = 5f;
    public float snapFactor = 5f;
    Vector2 direction;
    Vector2 originalPos;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
        direction = startDirection;
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        if ((Mathf.Abs(pos.x - originalPos.x) > distance/2)
            | (Mathf.Abs(pos.y - originalPos.y) > distance / 2))
        {
            direction = -startDirection;
        }
        else
        {
            direction = startDirection;
        }
        if (rb.velocity.x < maxSpeed && rb.velocity.y < maxSpeed)
        {
            rb.AddForce(direction * snapFactor);
        }
    }

}
