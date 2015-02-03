using UnityEngine;
using System;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelPack))]
public class LevelPackEditor : Editor 
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		LevelPack myTarget = (LevelPack)target;
		if (GUILayout.Button ("Re-Scan for levels")) {
			myTarget.scan();
		}
		if (GUI.changed) { 
			EditorUtility.SetDirty(myTarget);
		}

	}
}