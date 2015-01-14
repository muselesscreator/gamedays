using UnityEngine;
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
	bool coolingDown;
	bool isMoving = false;
	public bool isTurning = false;
	public Vector3 targetRotation;
	public float turnSpeed;

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

		if (h == 0f && v == 0f)
			return;

		if (theBoard.initializing)
			return;

		if (coolingDown){
			return;
		}

		Debug.Log ("Moving (" + h + ", " + v + ")");


		if (isWalking == false && Math.Abs (h+v) > 0) {
			Debug.Log ("MOVING TRUE");
			Vector2 newPosition = new Vector2();
			startTime = Time.time;
			Vector3 movement = new Vector3();
			newPosition = position;
			if (Math.Abs(h) == 1f) {
				newPosition.y += h;
				movement.Set(h, 0f, 0f);
			}
			else if (Math.Abs (v) == 1f) {
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
					if (reqsNotMet) {
						Debug.Log ("GO COOL DOWN");
						coolDown ();
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
						Debug.Log ("Making a pathlight at " + position.ToString());
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
					StartCoroutine("coolDown");
					Debug.Log ("????????????????");

					negAction ();
				}
			}
			else {
				Debug.Log ("Tried to move outside the grid to " + newPosition.x + ", " + newPosition.y);
				StartCoroutine("coolDown");
				Debug.Log ("????????????????????");
				negAction();
			}
			if (isWalking) {
				movement *= theBoard.size;
				targetPosition = movement + transform.position;
			}
		}
	}
	

	IEnumerator coolDown() {
		print ("Cooling Down");
		coolingDown = true;
		yield return new WaitForSeconds(0.5f);
		print("Cooled Down");
		coolingDown = false;
	}


	void negAction() {
		anim.SetTrigger ("negAction");
	}

	void FixedUpdate() {
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		if (isTurning == true) {
			Debug.Log ("Turning " +
						Vector3.Distance (transform.localEulerAngles, targetRotation) +
			           " from " +
			           transform.localEulerAngles.ToString() + 
			           " to " + 
			           targetRotation.ToString ());
			Debug.Log ("Distances: " +
			           Vector3.Distance (transform.localEulerAngles, new Vector3(0, targetRotation.y - 360f, 0)) +
			           " " + Vector3.Distance (transform.localEulerAngles, targetRotation) + 
			           " " + 
			           Vector3.Distance (transform.localEulerAngles, new Vector3(0, targetRotation.y + 360f, 0)));

			if ((Vector3.Distance (transform.localEulerAngles, new Vector3(0, targetRotation.y - 360f, 0)) <= 10) ||
			    (Vector3.Distance (transform.localEulerAngles, targetRotation) <= 10) ||
			    (Vector3.Distance (transform.localEulerAngles, new Vector3(0, targetRotation.y + 360f, 0)) <= 10)) {
				isTurning = false;
				Debug.Log ("Done rotating to " + transform.localEulerAngles.ToString ());

				transform.localEulerAngles = targetRotation;
				if (targetRotation.y == 360f) {
					transform.localEulerAngles = Vector3.zero;
				}
				else if (targetRotation.y == -90f) {
					transform.localEulerAngles = new Vector3(0, 270f, 0);
				}
			}
			else {
				float distCovered = (Time.time - startTime) * speed * 2;
				float fracJourney = distCovered / 90f;
				transform.localEulerAngles = Vector3.Lerp (transform.localEulerAngles, targetRotation, turnSpeed * Time.deltaTime);
			}
		}
		else if (coolingDown) {
			return;
		}
		else if (Math.Abs (h) >0) {
			float rotation = transform.localEulerAngles.y + 90*h;

			targetRotation = new Vector3(0, rotation, 0);
			isTurning = true;
			startTime = Time.time;
			StartCoroutine("coolDown");
		}
		else if (isWalking == true) {
			Debug.Log ("IsWalking from " + transform.position.ToString() + " to " + targetPosition.ToString ());
			Debug.Log (Vector3.Distance (transform.position, targetPosition));
			if (Vector3.Distance (transform.position, targetPosition) <= .01) {
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
		else if (v == 1f) {
			Debug.Log ("Not walking and not moving");
			if (Math.Abs ((int)transform.localEulerAngles.y) <.1) {
				Move (0f, 1f);
			}
			if (Math.Abs ((int)transform.localEulerAngles.y - 90) < 0.1) {
				Move (1f, 0f);
			}
			if (Math.Abs ((int)transform.localEulerAngles.y - 180) < 0.1) {
				Move (0f, -1f);
			}
			if (Math.Abs ((int)transform.localEulerAngles.y - 270) < 0.1) {
				Move (-1f, 0f);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("LookHorizontal");
		float v = Input.GetAxis("LookVertical");

		//Debug.Log ("Input = (" + h + ", " + v + ")");
	}
}
