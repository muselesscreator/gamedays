using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour {

	public int[] position = new int[2] {0,0};
	public Board theBoard;
	public bool isWalking = false;
	public Vector3 targetPosition;
	Animator anim;
	public float speed;
	public float startTime;
	public float journeyLength;
	// Use this for initialization
	
	void Start () {
		theBoard = GameObject.Find ("GameController").GetComponent<Board>();
		anim = GetComponent<Animator>();
		journeyLength = theBoard.size;
	}

	void Move (float h, float v) {
		Debug.Log (h + " " + v);

		if (isWalking == false && Math.Abs (h+v) > 0) {
			startTime = Time.time;

			anim.SetBool("isWalking", true);

			if (Math.Abs(h) > 0f) {
				h = -h;
				if (position[0] - h >= 0 && position[0] - h < theBoard.rows) {
					isWalking = true;
					targetPosition.Set(h, 0f, 0f);
					position[0] -= (int) h;
				}
			}

			else if (Math.Abs (v) > 0f) {
				if (position[1] + v >= 0 && position[1] + v < theBoard.cols) {
					isWalking = true;
					targetPosition.Set(0f, 0f, v);
					position[1] += (int) v;
				}
			}
			targetPosition *= theBoard.size;
			targetPosition += transform.position;
		}
		if (isWalking) {
			if (transform.position == targetPosition) {
				anim.SetBool ("isWalking", false);
				isWalking = false;
			}
			else {
				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				transform.position = Vector3.Lerp (transform.position, targetPosition, fracJourney);
			}
		}
	}

	void FixedUpdate() {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Debug.Log (h + " " + v);

		Move (h, v);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
