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

	public Vector3 down;
	public Vector3 right;
	// Use this for initialization
	
	void Start () {
		theBoard = GameObject.Find ("GameController").GetComponent<Board>();
		anim = GetComponent<Animator>();
		journeyLength = theBoard.size;
		down = new Vector3 (0, 0, 90);
		right = new Vector3 (90, 0, 0);
		theBoard.activate = true;
	}



	void Move (float h, float v) {
		if (isWalking == false && Math.Abs (h+v) > 0) {
			int[] newPosition = new int[2];
			startTime = Time.time;
			Vector3 movement = new Vector3();
			if (Math.Abs(h) > 0f) {
				transform.rigidbody.MoveRotation(Quaternion.LookRotation (right * h));
				newPosition = new int[2] {position[0], position[1] + (int) h};
				movement.Set(h, 0f, 0f);
			}
			else if (Math.Abs (v) > 0f) {
				transform.rigidbody.MoveRotation(Quaternion.LookRotation (down * v));
				newPosition = new int[2] {position[0] + (int) v, position[1]};
				movement.Set(0f, 0f, v);

			}
			if (theBoard.isValid(newPosition)) {
				if (theBoard.getPanel (newPosition).myType == Panel.PanelType.Default) {
					isWalking = true;
					anim.SetBool("isWalking", true);
					position = newPosition;
				}
				else {
					Debug.Log ("Tried to go over a visited panel at " + newPosition[1] + ", " + newPosition[0]);
					negAction ();
				}
			}
			else {
				Debug.Log ("Tried to move outside the grid to " + newPosition[1] + ", " + newPosition[0]);
				negAction();
			}
			if (isWalking) {
				movement *= theBoard.size;
				targetPosition = movement + transform.position;
			}
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

	void negAction() {
		anim.SetTrigger ("negAction");
	}

	void FixedUpdate() {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Move (h, v);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
