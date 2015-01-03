using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Panel : MonoBehaviour {

	public enum PanelType {
		Default,
		PanelOne,
		PanelTwo
	}

	public PanelType myType = PanelType.Default;
	PanelType oldType;
	Board theBoard;

	// Use this for initialization
	void Start () {
		theBoard = GameObject.Find("GameController").GetComponent<Board>();
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

	// Update is called once per frame
	void Update () {
		if (oldType != myType) {
			Debug.Log (myType);
			setMat(myType);
			oldType = myType;
		}
	}
}
