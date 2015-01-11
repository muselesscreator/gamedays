using UnityEngine;
using System.Collections;
using System.Linq;

public class PathLight : MonoBehaviour {

	Board theBoard;
	Panel myPanel;

	public ParticleSystem tracLight;
	public ParticleSystem beacon;
	public ParticleSystem bigBeacon;

	// Use this for initialization
	void Start () {

	}

	public void init (Vector2 position, Quaternion rotation) {
		theBoard = GameObject.Find ("GameController").GetComponent<Board> ();
		transform.parent = theBoard.getPanel (position).gameObject.transform;
		myPanel = theBoard.getPanel (position).GetComponent<Panel> ();
		transform.localRotation = rotation;
		transform.localPosition = new Vector3 (0f, .5f, 0f);

		tracLight = GetComponentsInChildren<ParticleSystem> ().Where (p => p.name == "tracLight").ElementAt(0);
		beacon = GetComponentsInChildren<ParticleSystem> ().Where (p => p.name == "beacon").ElementAt(0);
		bigBeacon = GetComponentsInChildren<ParticleSystem> ().Where (p => p.name == "bigBeacon").ElementAt(0);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
