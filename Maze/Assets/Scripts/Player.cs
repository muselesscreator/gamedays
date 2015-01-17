using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public Vector2 position = new Vector2(0, 0);
	public Board theBoard;
	public bool isWalking = false;
	public Vector3 targetPosition;
	Animator anim;
	public float speed;
	public float startTime;
	public float journeyLength;
	public List<Vector2> path;

	Vector3 down = new Vector3 (0, 0, 90);
	Vector3 right = new Vector3 (90, 0, 0);

	// Use this for initialization
	

	void Start () {
		theBoard = GameObject.Find ("GameController").GetComponent<Board>();
		anim = GetComponent<Animator>();
		journeyLength = theBoard.size;
		theBoard.activate = true;
	}

	public void BeAtIJ(Vector2 p) {
		Vector2 actualLocation = theBoard.ToXY((int) p.x, (int) p.y);
		transform.localPosition = new Vector3(actualLocation.x, 0f, actualLocation.y);
		position = p;
	}

	public void Move (float h, float v) {
		if (theBoard.initializing)
			return;

		if (isWalking == false && Math.Abs (h+v) > 0) {
			Vector2 newPosition = new Vector2();
			startTime = Time.time;
			Vector3 movement = new Vector3();
			newPosition = position;
			if (Math.Abs(h) > 0f) {
				transform.rigidbody.MoveRotation(Quaternion.LookRotation (right * h));
				newPosition.y += h;
				movement.Set(h, 0f, 0f);
			}
			else if (Math.Abs (v) > 0f) {
				transform.rigidbody.MoveRotation(Quaternion.LookRotation (down * v));
				newPosition.x += v;
				movement.Set(0f, 0f, v);

			}
			if (theBoard.isValid(newPosition)) {
				Panel newPanel = theBoard.getPanel (newPosition);
				if (newPanel.triggered == false) {
					bool reqsNotMet = false;
					if (newPanel.myKey > 0) {
						for (int i=0; i < newPanel.myKey-1; i++) {
							if (!theBoard.numPanels[i].activated) {
								reqsNotMet = true;
							}
						}
						if (!reqsNotMet) {
							theBoard.getNumPanel(newPanel.myKey).activated = true;
						}
					}
					else if (newPanel.myKey == -1) {
						Debug.Log ("new panel is exit");
						for (int i=0; i<theBoard.numPanels.Length; i++) {
							NumPanel numPanel = theBoard.numPanels[i];
							if (numPanel.placed && !numPanel.activated) {
								reqsNotMet = true;
							}
						}
					}
					if (!reqsNotMet){
						// Actually Walking!
						isWalking = true;
						anim.SetBool("isWalking", true);
						GameObject pathLight = GameObject.Instantiate(Resources.Load("prefabs/PathLight")) as GameObject;
						pathLight.GetComponent<PathLight>().init(position, transform.rigidbody.rotation);

						if (theBoard.getPanel (position).myKey > 0) {
							pathLight.GetComponent<PathLight>().bigBeacon.emissionRate = 250f;
						}

						position = newPosition;

						theBoard.getPanel (position).lightUp ();
					}
				}
				else {
					Debug.Log ("Tried to go over a visited panel at " + newPosition.x + ", " + newPosition.y);
					negAction ();
				}
			}
			else {
				Debug.Log ("Tried to move outside the grid to " + newPosition.x + ", " + newPosition.y);
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
				if (position == theBoard.endTile.position) {
					theBoard.nextLevel();
				}
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
