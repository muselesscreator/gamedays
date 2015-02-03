using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LevelSelectFrame : MonoBehaviour {

	public GameObject pack_frame;
	LevelPackManager level_manager;

	public float label_offset = 25;
	public float last_y_offset;

	
	public float width = 431;
	public float main_label_offset = 25;
	public float pack_label_offset = 20;
	public float level_width = 30;
	public float level_padding = 12;

	// Use this for initialization
	void Start() {
		level_manager = GameObject.Find ("GameController").GetComponent<LevelPackManager> ();
		last_y_offset = -label_offset;
		for (int i=0; i < level_manager.levelPacks.Count; i++) {
			showPack(i);
		}
		RectTransform panel = GameObject.Find ("LevelSelectPanel").GetComponent<RectTransform> ();
	}

	void showPack(int index) {
		LevelPack pack = level_manager.levelPacks [index];
		GameObject frame = GameObject.Instantiate (pack_frame) as GameObject;

		frame.transform.parent = transform.FindChild ("LevelSelectPanel").transform;
		frame.GetComponent<LevelPackFrame> ().setup (pack);


		float h = frame.GetComponent<LevelPackFrame> ().getPackHeight ();

		Vector2 newPos = new Vector2(-8, last_y_offset - h - main_label_offset);
		frame.GetComponent<RectTransform> ().anchoredPosition = newPos;

		last_y_offset -= h + main_label_offset;

	}

	public float getPackHeight(LevelPack pack) {
		return label_offset + level_padding + ((level_width + level_padding) * (float)(Math.Floor (pack.levels.Count / 10f) + 1));
	}
	
	public float getTotalHeight() {
		float size = label_offset;
		for (int i = 0; i < level_manager.levelPacks.Count; i++) {
			size += getPackHeight (level_manager.levelPacks[i]);
		}
		return size;
	}   
	
	// Update is called once per frame
	void Update () {
	}
}
