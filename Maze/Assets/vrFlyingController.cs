using UnityEngine;
using System;
using System.Collections;

public class vrFlyingController : MonoBehaviour {

	GameObject thePlayer;
	public Vector3 velocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;

	// Use this for initialization
	void Start () {
		thePlayer = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Math.Abs(Input.GetAxis ("Horizontal")) != 0) {

		}
	}
}
