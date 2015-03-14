using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour {

	public bool activate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (activate) {
			activate = false;
			Application.CaptureScreenshot("screenshot.png", 10);
		}
	}
}
