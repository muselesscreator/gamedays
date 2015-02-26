using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;

public class SaveManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Save() {
		PlayerPrefs.SetInt ("FirstInt", 12);
		PlayerPrefs.Save();
	}

	public void Load() {
		Debug.Log (PlayerPrefs.GetInt ("FirstInt"));
	}
}
