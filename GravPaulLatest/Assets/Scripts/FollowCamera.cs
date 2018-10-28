using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform tgt;

    // Update is called once per frame
    void FixedUpdate () {
        float x = tgt.position.x;
        float y = tgt.position.y;
        transform.position = new Vector3(x, y, 0);
	}
}
