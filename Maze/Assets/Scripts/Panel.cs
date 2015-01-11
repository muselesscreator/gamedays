using UnityEngine;
using System.Collections;
using System;

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

	// Use this for initialization
	void Awake () {
		theBoard = GameObject.Find("GameController").GetComponent<Board>();
		thePlayer = GameObject.Find ("sammy").GetComponent<Player> ();
		visible = true;
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
			theBoard.startLocation = position;
			thePlayer.BeAtIJ(position);
			theBoard.initializing = false;
		}
		else if (theBoard.selecting) {

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
			clearDecorators ();
			addExit();
			theBoard.setEndPoint = false;
		}
		else if  (theBoard.addObstacle) {
			Debug.Log ("Add Obstacle");
			if (visible) {
				theBoard.obstacles.Add (position);
				makeInvisible();
			}
		}
		else if (theBoard.removeObstacle) {
			Debug.Log ("Remove Obstacle");
			if (!visible) {
				theBoard.obstacles.Remove (position) ;
				makeVisible();
			}
		}
		else {
			if (Vector2.Distance (position, thePlayer.position) ==1 ) {
				Vector2 movement = position - thePlayer.position;
				thePlayer.Move(movement.y, movement.x);
			}
		}
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
			Debug.Log ("Clearing exit");
			theBoard.endTile.position = Vector2.zero;
			theBoard.endTile.placed = false;
			Destroy (GameObject.Find ("PanelEnd"));
		}
	}
	
	public void addExit() {
		Debug.Log (theBoard);
		Debug.Log (theBoard.endTile);
		if (theBoard.endTile.placed) {
			Destroy(GameObject.Find ("PanelEnd"));
			theBoard.getPanel (theBoard.endTile.position).myKey = 0;
		}
		GameObject exit = GameObject.Instantiate (Resources.Load ("Prefabs/PanelEnd")) as GameObject;
		exit.name = "PanelEnd";
		exit.transform.parent = transform;
		exit.transform.localPosition = new Vector3(0, .5f, 0);
		theBoard.endTile.placed = true;
		theBoard.endTile.position = position;
		myKey = -1;
	}
	
	public void AddNumber(NumPanel numPanel) {
		string name = "Panel" + numPanel.number;
		GameObject number = GameObject.Instantiate (Resources.Load ("Prefabs/" + name)) as GameObject;
		number.name = name;
		number.transform.parent = transform;
		number.transform.localPosition = new Vector3 (0, .5f, 0);
		myKey = Int32.Parse (numPanel.number);
	}
	
	void OnMouseEnter() {
		Debug.Log ("Drag");
		if (Input.GetMouseButtonDown(0) && Vector2.Distance (position, thePlayer.position) ==1 ) {
			Vector2 movement = position - thePlayer.position;
			thePlayer.Move(movement.y, movement.x);
		}
	}
	
	void setMat(PanelType type) {
		switch (type) 
		{
			case PanelType.Default:
				renderer.sharedMaterial = theBoard.materials.panelDefault;
				break;
			case PanelType.PanelOne:
				renderer.sharedMaterial = theBoard.materials.panelOne;
				break;
			case PanelType.PanelTwo:
				renderer.sharedMaterial = theBoard.materials.panelTwo;
			break;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			triggered = true;
			Debug.Log ("Entered Panel");
		}
		//myType = PanelType.PanelOne;
	}

	// Update is called once per frame
	void Update () {
		if (oldType != myType) {
			Debug.Log (myType);
			setMat(myType);
			oldType = myType;
		}
	}
}
