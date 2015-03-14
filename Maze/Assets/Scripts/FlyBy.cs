using UnityEngine;
using System.Collections;

public class FlyBy : MonoBehaviour {

	public GameObject origin;
	public GameObject target;

	public Vector3 startPosition;
	public Vector3 endPosition;

	public float len = 2f;
	public float currentLerpTime = 0f;

	public bool moving = false;

	// Use this for initialization
	void Start () {
		Go ();
	}

	public void Go() {
		startPosition = transform.localPosition;
		endPosition = target.transform.localPosition;
		transform.parent.LookAt(endPosition);
		currentLerpTime = 0f;
		moving = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			Debug.Log (endPosition);
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime > len) {
				currentLerpTime = len;
			}
			
			float fracJourney = currentLerpTime / len;
			transform.parent.localPosition = Vector3.Lerp (startPosition, endPosition, fracJourney);
		}
	}
}
