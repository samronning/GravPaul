using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour {

    Animator anim;
    Collider2D myCol;

	void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("gravOrb"))
        {
            StartCoroutine(openDoor());
        }
    }

    IEnumerator openDoor()
    {
        anim = GetComponent<Animator>();
        myCol = GetComponent<Collider2D>();
        anim.SetBool("isOpening", true);
        myCol.enabled = false;
        yield return new WaitForSeconds(4f);
        anim.SetBool("isOpening", false);
        myCol.enabled = true;
    }
}
