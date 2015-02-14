using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PanelMaterials {
	public Material panelDefault;
	public Material dimPanel;
	public Material brightPanel;
}

//[ExecuteInEditMode]
public class Board : MonoBehaviour {
	/* The main class for generating/maintaining the Maze board
	 * 
	 * has a number of properties, which can be saved/loaded to/from BoardTemplates
	 * as well as a number of toggles, allowing the Level designer to tweak elements of the maze
	 * 
	 * Parameters:
	 *  * int m_width: number of panels wide
	 *  * int m_height: number of panels long
	 *  * float size: size (in unity3d units (meters)) of a panel
	 *  * Vector2 start: start location (in (i,j) units) of the player
	 *  * NumPanels[] numPanels: configuration/controls for the location of number panels
	 *  * List<Vector2> obstacles: locations for missing tiles
	 *  * BoardTemplate myTemplate: a BoardTemplate that can be loaded in and activated with .load
	 * 
	 * Toggles:
	 *  * activate: re-generates the board, clears number/obstacle tiles, and sets to initialize
	 *              (re-place the start point)
	 *  * save: opens a save prompt to save the current configuration as a BoardTemplate
	 *  * NumPanel.select: next time you click on a Panel in Play mode, that panel will be associated with 
	 *                     this numPanel and the associated visual will be displayed
	 *  * addObstacle: next time you click on a panel, it will be made invisible, and its location will be added as an obstacle
	 *  * removeObstacle: next time you click on an invisible tile, it will be made visible and removed from the obstacle list
	 *  * setEndPoint: next time you click on a tile, it will be selected as a new exit tile.
	 */ 
	public bool initializing;
	public bool activate = false;
	public BoardTemplate myTemplate;
	public bool loadTemplate;

	public static Board boardInstance;

	public bool save = false;

	public bool selecting;
	public int selecting_index;

	public EndTile endTile;
	public bool setEndPoint;

	
	public int m_height;
	public int m_width;
	public float size;
	public Vector2 startLocation;

	public NumPanel[] numPanels;

	public List<Vector2> obstacles;
	public bool addObstacle;
	public bool removeObstacle;

	public GameObject panel;
	public PanelMaterials materials;
	public GameObject[,] theBoard;
	public Player thePlayer;
	public LevelPackManager manager;


	// Use this for initialization
	void Start () {
		manager = GetComponent<LevelPackManager> ();
		if (boardInstance == null)
		{
			boardInstance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	public Vector2 ToXY(int i, int j) {
		float newX = size * ((1f - m_width) / 2f + j);
		float newY = size * ((1f - m_height) / 2f + i);
		return new Vector2 (newX, newY);
	}

	public void teleport() {
		getPanel (endTile.position).teleport ();
	}


	public void nextLevel() {
		string[] levelDesc = myTemplate.name.Split ('_');
		Debug.Log (myTemplate.name);
		int pack = Int32.Parse(levelDesc[0]);
		int level = Int32.Parse (levelDesc[1]);
		string fn = "BoardTemplates/" + pack.ToString () + "_" + (level+1).ToString();
		Debug.Log (fn);
		BoardTemplate next = Resources.Load (fn) as BoardTemplate;
		Debug.Log (next);
		if (ApplicationModel.pack.last_cleared_level < level) {
			ApplicationModel.pack.last_cleared_level = level;
		}

		if (next == null) {
			GameObject[] panels = GameObject.FindGameObjectsWithTag ("panel");
			foreach (GameObject thisPanel in panels) {
				DestroyImmediate (thisPanel);
			}
			Application.LoadLevel (2); 
			return;
		}
		Debug.Log (next);
		ApplicationModel.template = next;
		loadTemplate = true;
		activate = true;
	}
	/*
	public void SaveTemplate() {
		string path = EditorUtility.SaveFilePanel ("Create new Maze Level",
		                                           "Assets/Resources/BoardTemplates/", "default.asset", "asset");
		
		if (path == "")
			return;
		
		path = FileUtil.GetProjectRelativePath (path);
		BoardTemplate template = GenTemplate ();
		AssetDatabase.CreateAsset (template, path);
		AssetDatabase.SaveAssets ();
	}
	*/

	public void LoadTemplate(BoardTemplate template) {
		m_width = template.m_width;
		m_height = template.m_height;
		startLocation = template.startLocation;
		numPanels = template.numPanels.Clone () as NumPanel[];
		obstacles = template.obstacles;
		endTile = template.endTile;
	}

	public BoardTemplate GenTemplate() {
		BoardTemplate bt = BoardTemplate.CreateInstance <BoardTemplate>();
		bt.m_width = m_width;
		bt.m_height = m_height;
		bt.size = size;
		bt.startLocation = startLocation;
		bt.endTile = endTile;
		bt.numPanels = numPanels.Clone () as NumPanel[];
		for(int i=0; i<numPanels.Length; i++) {
			NumPanel numPanel = bt.numPanels[i];
			numPanel.activated = false;
			numPanel.placed = numPanels[i].placed;
		}
		bt.obstacles = new List<Vector2> (obstacles);;
		return bt;
	}

	public void Resize(int width, int height) {
		m_height = height;
		m_width = width;
	}

	public Panel getPanel(Vector2 position) {
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
				if (obstacles.FindAll(p=> p.Equals(position)).Count > 0){
					Debug.Log ("Tried to move onto an obstacle tile");
				}
				else {
					return true;
				}
			}
		}
		return false;
	}

	public NumPanel getNumPanel(int num) {
		return numPanels.Where(n => n.number == num.ToString ()).ElementAt(0);
	}

	public int TotalPanels() {
		return (m_width * m_height) - obstacles.Count;
	}

	void GenBoard() {
		myTemplate = ApplicationModel.template;

		Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera> ();
		int max_axis = Math.Max (myTemplate.m_width, myTemplate.m_height);
		if (max_axis <= 4) {
			cam.fieldOfView = 24;
		}
		else if (max_axis == 5) {
			cam.fieldOfView = 32;
		}
		else if (max_axis == 6) {
			cam.fieldOfView = 34;
		}
		else if (max_axis == 7) {
			cam.fieldOfView = 38;
		}
		else if (max_axis == 8) {
			cam.fieldOfView = 44;
		}


		Destroy (GameObject.Find ("planet"));
		GameObject planet = GameObject.Instantiate (manager.Planets[ApplicationModel.pack.pack_num]) as GameObject;
		planet.name = "planet";
		planet.transform.position = new Vector3 (-1.5f, 0f, 12);

		thePlayer = GameObject.Find ("sammy").GetComponent<Player> ();

		if (loadTemplate) {
			LoadTemplate (myTemplate);
			thePlayer.BeAtIJ(startLocation);
		}
		else {
			initializing = true;
			obstacles = new List<Vector2> ();
			for (int i=0; i<numPanels.Length; i++) {
				NumPanel numPanel = numPanels[i];
				numPanel.position = Vector2.zero;
				numPanel.activated = false;
				numPanel.select = false;
				numPanel.placed = false;
				numPanel.number = (i+1).ToString();
			}
			endTile = new EndTile();
		}
		selecting = false;

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

		if (loadTemplate) {
			getPanel (startLocation).lightUp();
		}

		for (int i=0; i<numPanels.Length; i++) {
			NumPanel numPanel = numPanels[i];
			if (numPanel.placed) {
				numPanel.GetPanel ().AddNumber (numPanel);
				numPanel.activated = false;
			}
		}

		foreach (Vector2 obstacle in obstacles) {
			getPanel(obstacle).GetComponent<Panel>().makeInvisible();
		}
		if (endTile.placed) {
			getPanel(endTile.position).addExit();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activate || Input.GetButtonDown ("Jump")) {
			activate = false;
			GenBoard ();
		}
		if (save) {
			save = false;
			//SaveTemplate();
		}
		if (addObstacle) {
			removeObstacle = false;
		}
		for (int i =0; i < numPanels.Length; i++) {
			NumPanel numPanel = numPanels[i];
			if (numPanel.select) {
				selecting_index = i;
				selecting = true;
			}
		}
	}
}
