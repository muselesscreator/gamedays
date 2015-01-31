using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	Vector3 lowState;
	Vector3 highState;
	Vector3 neutralState;
	float speed;
	public Texture texture;
	public string state = "Entering";
	public FishSpawner fishSpawner;

	// Use this for initialization
	void Start () {
		fishSpawner = gameObject.transform.parent.GetComponent<FishSpawner> ();
		lowState = new Vector3 (0, fishSpawner.lowY, 0);
		highState = new Vector3 (0, fishSpawner.highY, 0);
		neutralState = Vector3.zero;
		speed = fishSpawner.speed;
		gameObject.transform.localPosition = lowState;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (state);
		if (state == "Entering") {
			if (Vector3.Distance (transform.localPosition, highState) > .1) {
				transform.localPosition = Vector3.Lerp (gameObject.transform.localPosition, highState, speed*Time.deltaTime);
			}
			else {
				state = "BouncingBack";
			}
		}
		if (state == "BouncingBack") {
			if (Vector3.Distance(transform.localPosition, neutralState) > .1) {
				transform.localPosition = Vector3.Lerp (gameObject.transform.localPosition, neutralState, speed * Time.deltaTime);
			}
			else {
				state = "Done";
			}
		}

	}
}
