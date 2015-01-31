using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class DetectTouch : MonoBehaviour {

	int waterMask;
	float camRayLength = 2500f;
	LevelController levelController;
	Player thePlayer;
	// Use this for initialization
	void Start () {
		waterMask = LayerMask.GetMask ("Water");
		levelController = GameObject.Find ("GameController").GetComponent<LevelController> ();
		thePlayer = GameObject.Find ("GameController").GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (camRay, out hit, camRayLength, waterMask)) {
				Debug.Log (hit.transform.gameObject.name);
				if (hit.transform.gameObject.name == "Fish") {
					GameObject fish = hit.transform.gameObject;
					GameObject parent = hit.transform.parent.gameObject;

					if (fish.GetComponent<Fish>().texture == levelController.target.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture) {
						Debug.Log ("Matching color clicked");
						thePlayer.add_score (10);
						parent.GetComponent<FishSpawner>().removeFish();
						levelController.newFish();
						levelController.chooseTarget();
					}
				}
			}
		}
	}
	
	
}
