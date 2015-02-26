using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LevelPackFrame : MonoBehaviour {

	public LevelPack myPack;

	public int numLevels;
	public GameObject level_template;

	public float width = 431;
	public float main_label_offset = 25;
	public float label_offset = 20;
	public float level_width = 30;
	public float padding = 12;
	public float total_level_width;
	public int numRows;

	public LevelPackManager manager;
	public LevelSelectFrame select_frame;
	
	public void setup (LevelPack p) {
		myPack = p;
		numLevels = myPack.levels.Count;
		manager = GameObject.Find ("GameController").GetComponent<LevelPackManager> ();
		select_frame = GameObject.Find ("LevelSelectFrame").GetComponent<LevelSelectFrame> ();


		RectTransform rect = GetComponent<RectTransform> ();
		transform.localScale = Vector3.one;
		rect.anchoredPosition3D = new Vector3 (rect.anchoredPosition.x, rect.anchoredPosition.y, 0);
		GetComponentInChildren<Text> ().text = myPack.name;

		width = select_frame.width;
		label_offset = select_frame.pack_label_offset;
		level_width = select_frame.level_width;
		padding = select_frame.level_padding;
		

		total_level_width = level_width + padding;
		numRows = (int)Math.Ceiling(numLevels / 10f);

		resize ();

		int last_cleared = 0;
		if (PlayerPrefs.HasKey("Pack_" + myPack.name + "_last_cleared")) {
			last_cleared = PlayerPrefs.GetInt ("Pack_" + myPack.name + "_last_cleared");
		}
		for (int i=0; i < manager.levelPacks.Count; i++) {
			LevelPack pack = manager.levelPacks[i];
		}
		for (int i = 0; i < numLevels; i++) {
			GameObject newLevel = GameObject.Instantiate(level_template) as GameObject;
			if (i < last_cleared)
				newLevel.GetComponent<LevelSelector>().active = true;
			if (i <= last_cleared) 
				newLevel.GetComponent<LevelSelector>().unlocked = true;
			newLevel.transform.parent = transform.FindChild ("Levels").transform;
			newLevel.GetComponent<LevelSelector>().initialize(this, i);
		}
		GameObject planet = GameObject.Instantiate (manager.Planets[myPack.pack_num]) as GameObject;
		planet.transform.parent = transform;
		planet.transform.localScale = Vector3.one * 8f;
		planet.transform.localPosition = new Vector3 (310, getPackHeight()-50, -10);


	}


	public void resize() {
		RectTransform rect = GetComponent<RectTransform> ();
		Vector2 newSize = new Vector2 (rect.sizeDelta.x, getPackHeight());
		rect.sizeDelta = newSize;
	}

	public float getPackHeight() {
		Debug.Log ("MyPack: " + myPack.pack_num);
		return label_offset + padding + (level_width + padding) * numRows;
	}

	void Update () {

	}
}
