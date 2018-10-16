using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContr : MonoBehaviour {

    [SerializeField]
    private Vector2 gravityCoef = new Vector2(0, -9.81f);

	// Use this for initialization
	void Awake () {
        Physics2D.gravity = gravityCoef;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
