using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NumPanel {
	public string number;
	public Vector2 position;
	public bool select;
	public bool activated;
	public bool placed;

	public Panel GetPanel() {
		Board theBoard = GameObject.Find ("GameController").GetComponent<Board> ();
		Panel thePanel = theBoard.getPanel (position).GetComponent<Panel>();
		return thePanel;
	}
}

[System.Serializable]
public class EndTile {
	public Vector2 position;
	public bool placed;
}

public class BoardTemplate : ScriptableObject {

	public int m_width;
	public int m_height;
	public float size;
	public Vector2 startLocation;
	public List<Vector2> obstacles;
	public NumPanel[] numPanels;
	public EndTile endTile;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
