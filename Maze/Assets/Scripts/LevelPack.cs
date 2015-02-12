using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelPack : ScriptableObject{
	
	public LevelPack(int id, string m_name, GameObject m_planet, GameObject m_panel, List<BoardTemplate> templates) {
		name = m_name;
		planet = m_planet;
		tiles = m_panel;
		pack_num = id;
		levels = templates;
	}

	[SerializeField]
	public string name;

	[SerializeField]
	public int pack_num;

	[SerializeField]
	public GameObject planet;
	[SerializeField]
	public GameObject tiles;
	[SerializeField]
	public List<BoardTemplate> levels;

	[SerializeField]
	public bool unlocked = false;
	[SerializeField]
	public int last_cleared_level = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	

	public void scan() {
		int level = 0;
		levels = new List<BoardTemplate> ();
		while (true) {
			level ++;
			string fn = "BoardTemplates/" + pack_num.ToString () + "_" + (level).ToString();
			BoardTemplate next = Resources.Load (fn) as BoardTemplate;
			if (next == null) {
				break;
			}
			else {
				levels.Add(next);
			}
		}
	}
}
