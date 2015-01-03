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

	// Use this for initialization
	void Start () {
	
	}

	void makeWall(float x, float z, Vector3 rotation) {
		for (int i=0; i<wallHeight; i++) {
			GameObject newPanel = GameObject.Instantiate (panel) as GameObject;
			float newY = size/2f + i*size;
			Debug.Log(newY);
			newPanel.transform.parent = gameObject.transform;
			newPanel.transform.localPosition = new Vector3(x, newY, z);
			newPanel.transform.localRotation = Quaternion.Euler(rotation);
		}
	}

	// Update is called once per frame
	void Update () {
		if (activate) {
			activate = false;
			GameObject[] panels = GameObject.FindGameObjectsWithTag ("panel");
			foreach (GameObject thisPanel in panels) {
				DestroyImmediate (thisPanel);
			}
			Vector3 rotation = new Vector3(0, 0, 90);
			for (int i=0; i<rows; i++) {
				float newX = -size*cols/2f;
				float newZ = size*(i+(1-rows)/2f);
				makeWall (newX, newZ, rotation);
				for (int j=0; j<cols; j++) {
					GameObject newPanel = GameObject.Instantiate(panel) as GameObject;
					newPanel.transform.parent = gameObject.transform;
					newPanel.transform.localPosition = new Vector3(size*((1-cols)/2f + j), 0, size*((1-rows)/2f + i));
				}
			}
			rotation = new Vector3(90, 0, 0);
			for (int j=0; j<cols; j++) {
				float newZ = -size*rows/2f;
				float newX = size*(j+(1-cols)/2f);
				makeWall (newX, newZ, rotation);
			}
		}
	}
}
