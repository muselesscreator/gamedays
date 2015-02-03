using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelPackManager : MonoBehaviour {

	public List<LevelPack> levelPacks;
	LevelPack currentPack;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadNewPack(int id, string name, GameObject planet, GameObject panel) {
		int level = 0;
		List<BoardTemplate> templates = new List<BoardTemplate> ();
		while (true) {
			level ++;
			string fn = "BoardTemplates/" + id.ToString () + "_" + (level).ToString();
			BoardTemplate next = Resources.Load (fn) as BoardTemplate;
			if (next == null) {
				break;
			}
			else {
				templates.Add(next);
			}
		}
		LevelPack newPack = new LevelPack (id, name, planet, panel, templates);
		levelPacks.Add (newPack);
	}
}
