using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[ExecuteInEditMode]
public class Panel : MonoBehaviour {

	public enum PanelType {
		Default,
		PanelOne,
		PanelTwo
	}

	public Vector2 position;
	public PanelType myType = PanelType.Default;
	PanelType oldType;
	public bool triggered = false;
	public int myKey = 0;
	Board theBoard;
	Player thePlayer;
	public bool visible;
	public PathLight pathLight;
	public GameObject myTile;
	public LevelPackManager manager;
	public GameObject numPanel = null;
	public float teleport_time = 0f;


	// Use this for initialization
	void Awake () {
		theBoard = GameObject.Find("GameController").GetComponent<Board>();
		manager = GameObject.Find ("GameController").GetComponent<LevelPackManager> ();
		thePlayer = GameObject.Find ("sammy").GetComponent<Player> ();
		pathLight = transform.GetComponentInChildren<PathLight>();
		myTile = GameObject.Instantiate (manager.Panels.Where (p => p.name == ApplicationModel.pack.tiles.name).ElementAt(0)) as GameObject;
		myTile.transform.parent = transform;
		myTile.name = ApplicationModel.pack.tiles.name;
		RectTransform rect = myTile.GetComponent<RectTransform> ();
		rect.anchoredPosition3D = Vector3.zero;
		visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (numPanel != null) {
			numPanel.transform.Rotate (new Vector3(0, 5, 0));
		}
		if (teleport_time > 0 && Time.time > teleport_time) {
			theBoard.nextLevel();
		}
	}
	public void makeVisible() {
		visible = true;
		GetComponentInChildren<MeshRenderer> ().enabled = true;
		ParticleSystem[] parts = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem part in parts) {
			part.enableEmission = true;
		}
	}
	public void makeInvisible() {
		Debug.Log ("Make Invisible");
		visible = false;
		GetComponentInChildren<MeshRenderer> ().enabled = false;
		ParticleSystem[] parts = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem part in parts) {
			part.enableEmission = false;
		}
	}
	
	void OnMouseDown() {
		Debug.Log ("Clicked");

		if (theBoard.initializing) {
			/*
			 * This means that the board is starting up, and we are selecting start.
			 */
			theBoard.startLocation = position;
			thePlayer.BeAtIJ(position);
			lightUp ();
			theBoard.initializing = false;
		}
		else if (theBoard.selecting) {
			/*
			 * Here, we are selecting this panel to be a number panel
			 */
			NumPanel numPanel = theBoard.numPanels [theBoard.selecting_index];
			string name = "Panel" + numPanel.number;

			numPanel.GetPanel ().myKey = 0;
		
			//clear all instances of this number
			Destroy (GameObject.Find (name));

			clearDecorators();

			numPanel.position = position;
			numPanel.placed = true;

			theBoard.selecting = false;
			numPanel.select = false;

			AddNumber (numPanel);
		}
		else if (theBoard.setEndPoint) {
			/* 
			 * Set this panel as the end point for a level
			 */
			clearDecorators ();
			addExit();
			theBoard.setEndPoint = false;
		}
		else if  (theBoard.addObstacle) {
			/*
			 * Set this panel as an obstacle
			 */
			Debug.Log ("Add Obstacle");
			if (visible) {
				theBoard.obstacles.Add (position);
				makeInvisible();
			}
		}
		else if (theBoard.removeObstacle) {
			/*
			 * Remove this panel as an obstacle
			 */
			Debug.Log ("Remove Obstacle");
			if (!visible) {
				theBoard.obstacles.Remove (position) ;
				makeVisible();
			}
		}
		else {
			/* 
			 * game is running.  Try to move?
			 */
			if (!thePlayer.selected && !thePlayer.isWalking && thePlayer.position != position) { 
				thePlayer.tmp_path.Clear ();
				if (thePlayer.path.Contains (position)) {
					Debug.Log ("I've SEEN THIS");
					thePlayer.retraceTo(position);
					thePlayer.selected = true;
					thePlayer.selecting = false;
				}
				else if (thePlayer.canReach (position)) {
					thePlayer.retracing = false;
					thePlayer.select(position);
					thePlayer.selecting = true;
					thePlayer.selected = false;
					thePlayer.lastSelected = position;
				}
				else if (thePlayer.path.Contains(position)) {
					thePlayer.retraceTo(position);
				}
			}
			/*
			if (Vector2.Distance (position, thePlayer.position) ==1 ) {
				Vector2 movement = position - thePlayer.position;
				thePlayer.Move(movement.y, movement.x);
			}
			*/
		}
	}

	void OnMouseOver() {
		if (Input.GetMouseButton(0) && !thePlayer.isWalking) {
			if (thePlayer.lastSelected == position) 
				return;
			if (thePlayer.canReach(position) && thePlayer.selecting) {
				thePlayer.lastSelected = position;
				thePlayer.select(position);
			}
			else {
				thePlayer.selecting = false;
				thePlayer.selected = true;
				thePlayer.lastSelected = new Vector2();
			}
		}
	}

	void onMouseExit() {
		Debug.Log ("Mouse UP!");
		thePlayer.selecting = false;
		thePlayer.selected = true;
		thePlayer.lastSelected = new Vector2();
	}

	public void clearDecorators() {
		Debug.Log ("Clear Decorators");
		//clear numbers and numPanel settings for other numbers at this location
		foreach (NumPanel nPanel in theBoard.numPanels) {
			if (nPanel.position == position ) {
				Destroy (GameObject.Find ("Panel" + nPanel.number));
				nPanel.position = new Vector2 (0, 0);
				nPanel.GetPanel ().myKey = 0;
				nPanel.placed = false;
			}
		}
		if (theBoard.endTile.position == position) {
			removeExit ();
		}
	}

	
	public void addExit() {
		Debug.Log ("Add Exit");
		Debug.Log (theBoard);
		Debug.Log (theBoard.endTile);
		if (theBoard.endTile.placed) {
			Panel endPanel = theBoard.getPanel (theBoard.endTile.position);
			endPanel.removeExit();
			theBoard.getPanel (theBoard.endTile.position).myKey = 0;
		}
		Destroy (myTile);
		myTile = GameObject.Instantiate (manager.Panels.Where (p => p.name=="exitTile").ElementAt(0)) as GameObject;
		myTile.transform.parent = transform;
		myTile.transform.localPosition = new Vector3(0, .5f, 0);
		theBoard.endTile.placed = true;
		theBoard.endTile.position = position;
		myKey = -1;
	}


	public void removeExit() {
		myKey = 0;
		Destroy (myTile);
		myTile = GameObject.Instantiate (manager.Panels.Where (p => p.name == ApplicationModel.pack.tiles.name).ElementAt(0)) as GameObject;
		myTile.transform.parent = transform;
		RectTransform rect = myTile.GetComponent<RectTransform> ();
		rect.anchoredPosition3D = Vector3.zero;
	}

	public void teleport() {
		Debug.Log ("teleport!!!");
		myTile.GetComponent<Animator> ().SetTrigger ("teleport");
		teleport_time = Time.time + 1.3f;
	}
	
	public void AddNumber(NumPanel nPanel) {
		string name = "number" + nPanel.number;
		numPanel = GameObject.Instantiate (manager.NumPanels.Where(n => n.name == name).ElementAt(0)) as GameObject;
		numPanel.GetComponent<Animator> ().SetInteger ("idleAction", UnityEngine.Random.Range (1, 8));
		numPanel.name = name;
		numPanel.transform.parent = transform;
		numPanel.transform.localPosition = new Vector3 (0, 1.5f, 0);
		numPanel.transform.localRotation = Quaternion.Euler (0, 300, 0);
		myKey = Int32.Parse (nPanel.number);
	}

	public bool isAdjacentTo(Vector2 newPos) {
		return (Vector2.Distance (position, newPos) == 1);		
	}

	/*
	void OnMouseEnter() {
		Debug.Log ("Drag");
		if (Input.GetMouseButtonDown(0) && Vector2.Distance (position, thePlayer.position) ==1 ) {
			Vector2 movement = position - thePlayer.position;
			thePlayer.Move(movement.y, movement.x);
		}
	}
	*/

	public void lightUp() {
		Material[] mats = GetComponentInChildren<MeshRenderer>().materials;
		if (myTile.name == "tile5") {
			mats[0] = theBoard.materials.brightPanel;
		}
		else {
			mats[1] = theBoard.materials.brightPanel;
		}
		GetComponentInChildren<MeshRenderer>().materials = mats;
	}

	public void goDark() {
		Debug.Log ("Go Dark at " + position.ToString ());
		Material[] mats = GetComponentInChildren<MeshRenderer>().materials;
		if (myTile.name == "tile5") {
			mats[0] = theBoard.materials.dimPanel;
		}
		else {
			mats[1] = theBoard.materials.dimPanel;
		}
		GetComponentInChildren<MeshRenderer>().materials = mats;
		pathLight.tracLight.enableEmission = false;
		pathLight.bigBeacon.enableEmission = false;
		IEnumerable<NumPanel> numPanels = theBoard.numPanels.Where (n => Vector2.Distance (n.position, position) == 0);
		if (numPanels.Count() > 0) {
			numPanels.ElementAt(0).activated = false;
		}

	}


	public void showPath(Quaternion rotation) {
		pathLight.tracLight.enableEmission = true;
		pathLight.transform.localRotation = rotation;
	}

	public void hidePath() {
		pathLight.tracLight.enableEmission = false;
	}

}
