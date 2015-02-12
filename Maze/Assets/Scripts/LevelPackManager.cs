using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using System.Linq;
using UniSave;

[System.Serializable]
public class LevelPackManager : MonoBehaviour {

	public List<LevelPack> levelPacks;
	LevelPack currentPack;
	
	public GameObject[] Panels;
	public GameObject[] Planets;
	public GameObject[] NumPanels;
	public AudioClip[] LevelMusic;

	private string _saveName = string.Empty;
	private Metafile[] _saves;
	private int _selectedSaveIndex;
	private string[] _saveNames;

	void Awake() {
		RefreshList();
		Load ();
		DontDestroyOnLoad(transform.gameObject);
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Cancel") > 0) {
			if (Application.loadedLevel == 0) {
			Debug.Log ("Exit");
			Application.Quit();
			}
			else {
				GameObject[] panels = GameObject.FindGameObjectsWithTag ("panel");
				foreach (GameObject thisPanel in panels) {
					DestroyImmediate (thisPanel);
				}
				DestroyImmediate (GameObject.Find ("sammy"));
				Application.LoadLevel(0);
			}
		}
	}

	public void RefreshList()
	{
		_saves = UniSave.SaveManager.GetSavesList();
		_saveNames = new string[_saves.Length];
		
		for (int i = 0; i < _saves.Length; i++)
		{
			Metafile save = _saves[i];
			_saveNames[i] = save.Name;
		}
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

	public void Save()
	{
		Debug.Log ("Save");
		UniSave.Components.SaveableObject s = GetComponent<UniSave.Components.SaveableObject> ();
		SaveManager.SaveObject ("playerInfo.dat", gameObject);
	}

	public void Load() {
		SaveManager.Load ("playerInfo.dat");
	}
}

[Serializable]
class PackData
{
	public int[] unlocked_packs;
	public int[] last_unlocked_level;
}

