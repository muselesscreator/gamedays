using UnityEngine;
using System.Collections;

public class BoardPlane : MonoBehaviour {
	public GameObject[,] theBoard;
	public Player thePlayer;

	// Use this for initialization
	void Start () {
		thePlayer = GameObject.Find ("sammy").GetComponent<Player> ();
	}
	
	void OnMouseOver() {
		if (Input.GetMouseButton(0)) {
			thePlayer.selecting = false;
			thePlayer.selected = true;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
