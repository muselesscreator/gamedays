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
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);
	}
}
