using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public int score = 0;
	public int strikes = 0;

	public Text scoreText;
	// Use this for initialization
	void Start () {
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
	}

	public void add_score(int points) {
		score = score + points;
		scoreText.text = score.ToString();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
