using UnityEngine;
using System.Collections;

public class MeteorGo : MonoBehaviour {

	public float speed=20;

	public float lifetime = 10;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().velocity = transform.forward * -speed;
		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

