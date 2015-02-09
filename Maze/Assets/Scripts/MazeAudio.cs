using UnityEngine;
using System.Collections;

public class MazeAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioSource src = GetComponent<AudioSource> ();
		int pack_num = ApplicationModel.pack.pack_num;
		src.clip = GameObject.Find ("GameController").GetComponent<LevelPackManager> ().LevelMusic [pack_num];
		src.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
