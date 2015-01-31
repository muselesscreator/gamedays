using UnityEngine;
using System.Collections;

public class RotateBoard : MonoBehaviour {

	public float speed = .2f;
	GameObject fishPlane;

	// Use this for initialization
	void Start () {
		fishPlane = GameObject.Find("FishPlane");
	}

	// Update is called once per frame
	void Update () {
		fishPlane.transform.Rotate(Vector3.up, speed*Time.deltaTime);
	}
}
