using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelPackManager))]
public class LevelPackManagerEditor : Editor 
{
	public bool addingLevelPack;
	public int currentID;
	public GameObject currentPlanet;
	public GameObject currentPanel;
	public string currentName;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		LevelPackManager myTarget = (LevelPackManager)target;

		if (GUILayout.Button ("New LevelPack")) {
			addingLevelPack = true;
		}

		if (addingLevelPack) {
			currentID = EditorGUILayout.IntField("Level Pack ID", currentID);
			currentPanel = EditorGUILayout.ObjectField("Panel", currentPanel, typeof(GameObject)) as GameObject;
			currentName = EditorGUILayout.TextField("Name", currentName);
			
			if (GUILayout.Button ("Save Level Pack")) {
				myTarget.loadNewPack (currentID, currentName, currentPlanet, currentPanel);
				addingLevelPack = false;
				currentID = 0;
				currentPanel = null;
				currentPlanet = null;
			}
		}
	}
}