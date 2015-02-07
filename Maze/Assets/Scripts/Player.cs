using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

	public Vector2 position = new Vector2(0, 0);
	public Board theBoard;
	public bool isWalking = false;
	public Vector3 targetPosition;
	Animator anim;
	public float speed;
	public float startTime;
	public float journeyLength;

	// true if we are currently selecting a path
	public bool selecting;
	// true if we have just finished selecting a path
	public bool selected;
	// true if we are currently moving backwards (retracing steps)
	public bool retracing;
	// vector for the last position actively selected
	public Vector2 lastSelected;

	// the index in the current tmp_path that we are moving to
	public int moveIndex;
	
	public List<Vector2> path;
	public List<Vector2> tmp_path;

	Vector3 down = new Vector3 (0, 0, 90);
	Vector3 right = new Vector3 (90, 0, 0);

	// Use this for initialization
	
	void Awake () {
		theBoard = GameObject.Find ("GameController").GetComponent<Board> ();
		anim = GetComponent<Animator> ();
	}
	
	void Start() {
		theBoard.loadTemplate = true;
		theBoard.activate = true;
	}

	public bool canReach(Vector2 newPos) {
		Debug.Log ("Can reach");
		Panel panel= theBoard.getPanel (newPos);
		if (tmp_path.Count == 0) {
			tmp_path.Add (position);
		}
		if (!panel.isAdjacentTo(tmp_path[tmp_path.Count-1])) {
			Debug.Log (newPos.ToString () + " is not adjacent to end of path");
			return false;
		}
		if (tmp_path.Contains(newPos) || path.Contains (newPos)) {
			Debug.Log ("Already hit this position");
			return false;
		}
		if (!couldReach (newPos)) {
			Debug.Log ("Requirements not satisfied");
			return false;
		}
		else {
			return true;
		}
	}

	public void select(Vector2 newPos) {
		Vector2 lastPos = tmp_path[tmp_path.Count - 1];
		tmp_path.Add (newPos);
		Vector2 diff = newPos - lastPos;
		theBoard.getPanel (lastPos).showPath(Quaternion.LookRotation(new Vector3(90*diff.y, 0, 90*diff.x)));
	}

	public bool couldReach(Vector2 newPos) {
		Panel panel= theBoard.getPanel (newPos);
		bool reqsNotMet = false;
		if (theBoard.obstacles.Contains(newPos)){
			Debug.Log ("This is an obstacle");
			return false;
		}
		if (panel.myKey > 0) {
			for (int i=0; i < panel.myKey-1; i++) {
				if (!theBoard.numPanels[i].activated && !tmp_path.Contains (theBoard.numPanels[i].position)) {
					reqsNotMet = true;
				}
			}
			if (reqsNotMet) {
				Debug.Log ("Previous number panels have not been activated");
				return false;
			}
		}
		else if (panel.myKey == -1) {
			return ((tmp_path.Count + path.Count) == theBoard.TotalPanels());
		}
		return true;
	}
	
	public void retraceTo(Vector2 position) {
		/*
		 * Retrace the path back to the input position
		 */
		int c = path.Count-1;
		tmp_path.Clear();
		retracing = true;
		for (int i=c; i >= path.IndexOf(position); i--) {
			tmp_path.Add (path[i]);
		}
		Move ();
	}


	public void BeAtIJ(Vector2 p) {
		Vector2 actualLocation = theBoard.ToXY((int) p.x, (int) p.y);
		transform.localPosition = new Vector3(actualLocation.x, 0f, actualLocation.y);
		position = p;
		path.Clear ();
		path.Add (position);
	}


	public void moveToNext() {
		Debug.Log ("MoveToNext " + moveIndex + " " + tmp_path.ToString ());
		Vector2 newPosition = tmp_path[moveIndex];
		startTime = Time.time;

		transform.localRotation = theBoard.getPanel (position).pathLight.transform.localRotation;

		// Actually Walking!
		isWalking = true;
		anim.SetBool("isWalking", true);

		if (retracing) {
			theBoard.getPanel (position).goDark ();
		}

		position = newPosition;

		if (!retracing) {
			theBoard.getPanel (position).lightUp ();
		}
		Vector2 xyPos = theBoard.ToXY ((int) newPosition.x, (int) newPosition.y);
		targetPosition = new Vector3(xyPos.x, 0, xyPos.y);
	}

	public void Move () {
		moveIndex = 1;
		moveToNext ();
	}

	void negAction() {
		anim.SetTrigger ("negAction");
	}

	void FixedUpdate() {
		if ((selected || (Input.GetMouseButton (0) == false && selecting)) && !retracing) {
			selected = false;
			selecting = false;	
			if (tmp_path.Count > 1 ) {
				Move ();
			}

		}
		if (isWalking) {
			if (transform.localPosition == targetPosition) {
				IEnumerable<NumPanel> numPanels = theBoard.numPanels.Where(n => n.placed && Vector2.Distance (n.position, tmp_path[moveIndex]) == 0);

				//If this panel has a number on it, and we are not retracing, then light up
				if (numPanels.Count() > 0 && !retracing) {
					numPanels.ElementAt(0).activated = true;
					theBoard.getPanel (tmp_path[moveIndex]).pathLight.bigBeacon.enableEmission = true;
				}
				// If we have reached the last location in our path
				if (moveIndex == tmp_path.Count-1) {
					isWalking = false;
					anim.SetBool("isWalking", false);
					if (position == theBoard.endTile.position) {
						theBoard.teleport();
					}
					for (int i=1; i<tmp_path.Count; i++) {
						if (retracing) {
							path.Remove(tmp_path[i-1]);
						}
						else {
							path.Add (tmp_path[i]);
						}
					}
					if (retracing) {
						theBoard.getPanel (position).pathLight.tracLight.enableEmission = false;
						retracing = false;
					}
					tmp_path.Clear ();

				}
				else {
					moveIndex += 1;
					moveToNext();
				}
			}
			else {
				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / theBoard.size;
				transform.localPosition = Vector3.Lerp (transform.localPosition, targetPosition, fracJourney);
			}
			
		}
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		//Move (h, v);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
