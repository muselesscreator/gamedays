using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class PanelMaterials {
	public Material panelDefault;
	public Material panelOne;
	public Material panelTwo;
}

[ExecuteInEditMode]
public class Board : MonoBehaviour {

	public bool activate = false;
	public GameObject panel;
	public int rows;
	public int cols;
	public float size;
	public int wallHeight;
	public PanelMaterials materials;
	public GameObject[,] theBoard;

	// Use this for initialization
	void Start () {
	
	}
	public Panel getPanel(int[] position) {
		if (position [1] >= 0 && position [1] < cols) {
			if (position[0] >= 0 && position[0] < rows) {
				return theBoard [position [0], position [1]].GetComponent<Panel> ();
			}
		}
		return null;
	}

	public bool isValid(int[] position) {
		if (position [1] >= 0 && position [1] < cols) {
			if (position[0] >= 0 && position[0] < rows) {
				return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		if (activate) {
			activate = false;
			GameObject[] panels = GameObject.FindGameObjectsWithTag ("panel");
			foreach (GameObject thisPanel in panels) {
				DestroyImmediate (thisPanel);
			}
			theBoard = new GameObject[cols, rows];
			for (int i=0; i<rows; i++) {
				float newX = -size*cols/2f;
				float newZ = size*(i+(1-rows)/2f);
				for (int j=0; j<cols; j++) {
					GameObject newPanel = GameObject.Instantiate(panel) as GameObject;
					theBoard[i, j] = newPanel;
					newPanel.transform.parent = gameObject.transform;
					newPanel.transform.localPosition = new Vector3(size*((1-cols)/2f + j), 0, size*((1-rows)/2f + i));
				}
			}

		}
	}
}
