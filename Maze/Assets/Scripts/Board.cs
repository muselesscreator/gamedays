using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[System.Serializable]
public class PanelMaterials {
	public Material panelDefault;
	public Material panelOne;
	public Material panelTwo;
}

[ExecuteInEditMode]
public class Board : MonoBehaviour {

	public bool activate = false;
	public bool save = false;
	public GameObject panel;
	public int m_height;
	public int m_width;
	public float size;
	public PanelMaterials materials;
	public GameObject[,] theBoard;
	public NumPanel[] numPanels;

	public int selecting_index;
	public bool selecting;

	// Use this for initialization
	void Start () {
	
	}

	public void SaveTemplate() {
		string path = EditorUtility.SaveFilePanel ("Create new Maze Level",
		                                           "Assets/BoardTemplates/", "default.asset", "asset");
		
		if (path == "")
			return;
		
		path = FileUtil.GetProjectRelativePath (path);
		BoardTemplate template = GenTemplate ();
		AssetDatabase.CreateAsset (template, path);
		AssetDatabase.SaveAssets ();
	}

	public BoardTemplate GenTemplate() {
		BoardTemplate bt = BoardTemplate.CreateInstance <BoardTemplate>();
		bt.m_width = m_width;
		bt.m_height = m_height;
		bt.size = size;
		bt.numPanels = numPanels;
		foreach (NumPanel numPanel in bt.numPanels) {
			numPanel.activated = false;
		}
		return bt;
	}

	public void Resize(int width, int height) {
		m_height = height;
		m_width = width;
	}

	public Panel getPanel(Vector2 position) {
		Debug.Log (position);
		//x and y are switched here... because
		if (position.y >= 0 && position.y < m_width) {
			if (position.x >= 0 && position.x < m_height) {
				return theBoard [(int)position.y, (int)position.x].GetComponent<Panel> ();
			}
		}
		return null;
	}

	public bool isValid(Vector2 position) {
		//x and y are switched here... because
		if (position.y >= 0 && position.y < m_width) {
			if (position.x >= 0 && position.x < m_height) {
				return true;
			}
		}
		return false;
	}

	public NumPanel getNumPanel(int num) {
		return numPanels.Where(n => n.number == num.ToString ()).ElementAt(0);
	}

	// Update is called once per frame
	void Update () {
		if (activate) {
			activate = false;
			GameObject[] panels = GameObject.FindGameObjectsWithTag ("panel");
			foreach (GameObject thisPanel in panels) {
				DestroyImmediate (thisPanel);
			}
			theBoard = new GameObject[m_width, m_height];
			for (int i=0; i<m_height; i++) {
				float newX = -size*m_width/2f;
				float newZ = size*(i+(1-m_height)/2f);
				for (int j=0; j<m_width; j++) {
					GameObject newPanel = GameObject.Instantiate(panel) as GameObject;
					theBoard[j, i] = newPanel;
					newPanel.transform.parent = gameObject.transform;
					newPanel.transform.localPosition = new Vector3(size*((1-m_width)/2f + j), 0, size*((1-m_height)/2f + i));
					newPanel.GetComponent<Panel>().position = new Vector2(i, j);
				}
			}

		}
		if (save) {
			save = false;
			SaveTemplate();
		}
		for (int i =0; i < numPanels.Length; i++) {
			NumPanel numPanel = numPanels[i];
			numPanel.number = (i+1).ToString();
			if (numPanel.select) {
				selecting_index = i;
				selecting = true;
			}
		}
	}
}
