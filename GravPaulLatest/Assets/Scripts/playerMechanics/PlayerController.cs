using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    bool firstTime;

    Animator playeranim;

    void Start()
    {
        firstTime = true;
    }

    protected override void FixedUpdate () {
        playeranim = GetComponent<Animator>();
        if (!isFiring)
        {
            GetInput();
            Animate();
        }
        base.FixedUpdate();
        groundCheck();
        wallDirection = base.wallCheck();
        shootGrav();
    }

    protected override void groundCheck()
    {
        Vector2 A = characterDrawOverlap()[0];
        Vector2 B = characterDrawOverlap()[1];
        Collider2D collisionGround = (Physics2D.OverlapArea(A, B, 1 << groundSurface));
        Collider2D collisionSpike = (Physics2D.OverlapArea(A, B, 1 << spikeSurface));

        if (collisionGround == null)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }

        if (collisionSpike != null && firstTime == true)
        {
            FindObjectOfType<audioManager>().Play("spikeDeath");
            StartCoroutine(Die());
            firstTime = false;
        }
    }

    private void GetInput()
    {

        //Input for Paul Movement
        direction = Vector2.zero;

        //GravDown
        if (Input.GetKey(KeyCode.A) && Physics2D.gravity.y < 0 && wallDirection != Vector2.left)
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D) && Physics2D.gravity.y < 0 && wallDirection != Vector2.right)
        {
            direction += Vector2.right;
        }

        //GravUp
        if (Input.GetKey(KeyCode.A) && Physics2D.gravity.y > 0 && wallDirection != Vector2.left)
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.D) && Physics2D.gravity.y > 0 && wallDirection != Vector2.right)
        {
            direction += Vector2.left;
        }

        //GravLeft
        if (Input.GetKey(KeyCode.W) && Physics2D.gravity.x < 0 && wallDirection != Vector2.up)
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S) && Physics2D.gravity.x < 0 && wallDirection != Vector2.down)
        {
            direction += Vector2.right;
        }

        //GravRight
        if (Input.GetKey(KeyCode.W) && Physics2D.gravity.x > 0 && wallDirection != Vector2.up)
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S) && Physics2D.gravity.x > 0 && wallDirection != Vector2.down)
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

    IEnumerator Die()
    {
        
        isFiring = true;
        direction = Vector2.zero;

        float killTime = FindObjectOfType<audioManager>().killMusic("level_1_music");
        yield return new WaitForSeconds(killTime);

        float fadeTime = GameObject.Find("GameManager").GetComponent<Fading>().beginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
