using UnityEngine;
using System.Collections;

public class TouchPlanet : MonoBehaviour {

	public LevelPackManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameController").GetComponent<LevelPackManager> ();
		GameObject planet = GameObject.Instantiate(manager.Planets[ApplicationModel.pack.pack_num]) as GameObject;
		planet.transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Application.LoadLevel (0);
	}
}
