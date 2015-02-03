using UnityEngine;
using System;
using System.Collections;

public class LevelSelector : MonoBehaviour {
	
	public BoardTemplate template;
	public int index;
	public Vector2 position;

	// Use this for initialization
	public void initialize (LevelPackFrame parent, int i) {
		gameObject.name = "LevelSelector";

		index = i;
		template = parent.myPack.levels [i];
		RectTransform myRect = GetComponent<RectTransform> ();
		myRect.sizeDelta = Vector2.one * parent.level_width;

		GetComponent<BoxCollider> ().size = new Vector3 (parent.level_width, parent.level_width, 1);
		GetComponent<BoxCollider> ().center = new Vector3 (parent.level_width / 2, parent.level_width / 2, 1);


		GameObject tile = GameObject.Instantiate (parent.myPack.tiles) as GameObject;
		tile.transform.parent = transform;
		RectTransform rect = tile.GetComponent<RectTransform> ();
		rect.anchoredPosition3D = new Vector3 (0, 0, -4.5f);
		tile.transform.localRotation = Quaternion.Euler (new Vector3 (0, 180, 0));
		tile.transform.localScale = Vector3.one * (parent.level_width/2);
		tile.layer = 5;

		transform.localScale = Vector3.one;
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, 0);

		int col =  (i % 10);
		int row = (int)Math.Floor(i / 10f);
		
		float newX = parent.padding + (parent.total_level_width * col);
		float newY = parent.padding + (parent.total_level_width * (parent.numRows - row - 1));

		GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, newY);
	}

	void OnMouseDown() {
		Debug.Log ("Clicked!");
		ApplicationModel.setTemplate (template);
		Application.LoadLevel (1);
	}

	public void Click() {
		//Debug.Log ("Clicked!");
	}

	// Update is called once per frame
	void Update () {

	}
}
