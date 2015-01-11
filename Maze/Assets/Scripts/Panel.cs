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

	// Use this for initialization
	void Start () {
		theBoard = GameObject.Find("GameController").GetComponent<Board>();
		thePlayer = GameObject.Find ("sammy").GetComponent<Player> ();
	}

	void OnMouseDown() {
		Debug.Log ("Clicked");
		if (theBoard.selecting) {

			NumPanel numPanel = theBoard.numPanels [theBoard.selecting_index];
			string name = "Panel" + numPanel.number;

			numPanel.GetPanel ().myKey = 0;
			numPanel.position = position;

			//clear all instances of this number
			Destroy (GameObject.Find (name));

			//clear numbers and numPanel settings for other numbers at this location
			foreach (NumPanel nPanel in theBoard.numPanels) {
					if (nPanel.position == position && nPanel.number != numPanel.number) {
							Destroy (GameObject.Find ("Panel" + nPanel.number));
							nPanel.position = new Vector2 (0, 0);
							nPanel.GetPanel ().myKey = 0;
					}
			}
			theBoard.selecting = false;
			numPanel.select = false;
			GameObject number = GameObject.Instantiate (Resources.Load ("Prefabs/" + name)) as GameObject;
			number.name = name;
			number.transform.parent = transform;
			number.transform.localPosition = new Vector3 (0, .5f, 0);
			myKey = Int32.Parse (numPanel.number);
		} else {
			if (Vector2.Distance (position, thePlayer.position) ==1 ) {
				Vector2 movement = position - thePlayer.position;
				thePlayer.Move(movement.y, movement.x);
			}
		}
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
