using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character {

    protected Vector2 shotDir;

    [SerializeField]
    private GameObject gravOrb;

    [SerializeField]
    private float fireRate = 1f;
    private float nextFire;

    [SerializeField]
    protected float gravityCoefficient = 20;

    private GameObject gravBall;

    bool isFiring = false;

    Animator playeranim;

    protected override void FixedUpdate () {
        playeranim = GetComponent<Animator>();
        if (!isFiring)
        {
            GetInput();
            Animate();
        }
        base.FixedUpdate();
        shootGrav();
    }

    private void GetInput()
    {

        //Input for Paul Movement
        direction = Vector2.zero;

        //GravDown
        if (Input.GetKey(KeyCode.A) && Physics2D.gravity.y < 0)
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D) && Physics2D.gravity.y < 0)
        {
            direction += Vector2.right;
        }

        //GravUp
        if (Input.GetKey(KeyCode.A) && Physics2D.gravity.y > 0)
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.D) && Physics2D.gravity.y > 0)
        {
            direction += Vector2.left;
        }

        //GravLeft
        if (Input.GetKey(KeyCode.W) && Physics2D.gravity.x < 0)
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) && Physics2D.gravity.x < 0)
        {
            direction += Vector2.right;
        }

        //GravRight
        if (Input.GetKey(KeyCode.W) && Physics2D.gravity.x > 0)
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S) && Physics2D.gravity.x > 0)
        {
            direction += Vector2.left;
        }

        //Input for Paul Shoot
        shotDir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            shotDir = Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            shotDir = Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            shotDir = Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            shotDir = Vector2.down;
        }

        if (isFiring) {
            direction = Vector2.zero;
            shotDir = Vector2.zero;
        }
    }

    void shootGrav()
    {
        if (shotDir.x != 0 | shotDir.y !=0 //Check if there is a shot direction
            && gravBall == null //Check if orb is gone yet
            && isGrounded //Check if player is in the air
            && Time.time > nextFire) //Apply FireRate
        {
            StartCoroutine(Firing());
            nextFire = Time.time + fireRate;
            gravBall = Instantiate<GameObject>(gravOrb, this.transform.position, Quaternion.identity);
            GravOrbContr orbcontr = gravOrb.GetComponent<GravOrbContr>();
            orbcontr.LaunchGrav(gravBall, shotDir);
        }
    }

    IEnumerator Firing()
    {
        //yield return new WaitForSeconds(0.01f);
        isFiring = true;
        yield return new WaitForSeconds(fireRate);
        isFiring = false;
        yield return null;
    }

    void Animate()
    {
        if (Physics2D.gravity.y != 0)
        {
            if (shotDir.x != 0)
            {
                playeranim.SetBool("isFiringRight", true);
            }
            if (shotDir.y != 0)
            {
                playeranim.SetBool("isFiringUp", true);
            }
        }

        if (Physics2D.gravity.x != 0)
        {
            if (shotDir.y != 0)
            {
                playeranim.SetBool("isFiringRight", true);
            }
            if (shotDir.x != 0)
            {
                playeranim.SetBool("isFiringUp", true);
            }
        }

        if (shotDir == Vector2.zero)
        {
            playeranim.SetBool("isFiringRight", false);
            playeranim.SetBool("isFiringUp", false);
        }
    }
}
