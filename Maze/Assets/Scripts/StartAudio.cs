using UnityEngine;
using System.Collections;

public class StartAudio : MonoBehaviour {

	public static StartAudio audioInstance;

	// Use this for initialization
	void Start () {
		if (audioInstance == null)
		{
			audioInstance = this;
		}
		else {
			Destroy(gameObject);
		}
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
