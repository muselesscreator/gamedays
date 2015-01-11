using UnityEngine;
using System.Collections;

[System.Serializable]
public class NumPanel {
	public string number;
	public Vector2 position;
	public bool select;
	public bool activated;

	public Panel GetPanel() {
		Board theBoard = GameObject.Find ("GameController").GetComponent<Board> ();
		Panel thePanel = theBoard.getPanel (position).GetComponent<Panel>();
		return thePanel;
	}
}

public class BoardTemplate : ScriptableObject {

	public int m_width;
	public int m_height;
	public float size;

	public NumPanel[] numPanels;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
