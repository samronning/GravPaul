﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    private float speed;

    //Ground Surfaces
    LayerMask groundSurface;

    protected bool isGrounded = false;

    protected Vector2 direction;

    private Vector2 gravChangeDir;

    void Awake()
    {
        groundSurface = LayerMask.NameToLayer("Ground");
    }
    protected virtual void Update()
    {
        Move();
        Vector2 A = characterDrawOverlap()[0];
        Vector2 B = characterDrawOverlap()[1];
        groundCheck(A , B);
        Flip();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Flip();
    }

    void groundCheck(Vector2 A, Vector2 B)
    {
        Collider2D collision = (Physics2D.OverlapArea(A, B, 1 << groundSurface));

        if (collision == null)
        {
            isGrounded = false;
            return;
        }
        else {
            isGrounded = true;
        }
    }

    Vector2[] characterDrawOverlap ()
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
            Vector2[] arrPoints = { altTopLeft, altBottomLeft };
            return arrPoints;
        }
        //Up
        if (Physics2D.gravity.y > 0)
        {
            Vector2[] arrPoints = { topLeft, topRight };
            return arrPoints;
        }
        //Down
        if (Physics2D.gravity.y < 0)
        {
            Vector2[] arrPoints = { bottomLeft, bottomRight };
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
}
