using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public int numFish = 8;
	public GameObject target;
	FishSpawner[] spawners;
	GameObject[] availableFish;
	GameObject[] emptySpawners;
	bool starting = true;
	public Texture[] textures;

	// Use this for initialization
	void Start () {
		spawners = GameObject.Find ("FishPlane").GetComponentsInChildren<FishSpawner>();
		target = GameObject.Find ("TargetFish");
		target.GetComponent<Animator> ().SetBool ("IsTarget", true);
	}

	public void chooseTarget() {
		Debug.Log ("Choose new target");
		availableFish = GameObject.FindGameObjectsWithTag("Fish");
		int index = Random.Range (0, availableFish.Length - 1);
		Debug.Log (index);
		GameObject targetFish = availableFish [index];
		Debug.Log (targetFish);
		Debug.Log (targetFish.GetComponent<Fish>().texture);
		target.GetComponentInChildren<SkinnedMeshRenderer> ().material.mainTexture = targetFish.GetComponent<Fish>().texture;
	}

	public void newFish() {
		int index = Random.Range (0, emptySpawners.Length-1);
		emptySpawners[index].GetComponent<FishSpawner>().addFish();
		emptySpawners[index].tag = null;
	}

	// Update is called once per frame
	void Update () {
		emptySpawners = GameObject.FindGameObjectsWithTag ("EmptySpawner");
		if ((spawners.Length - emptySpawners.Length) < numFish && starting) {
			newFish ();
		}
		else if (starting) {
			starting = false;
			chooseTarget ();
		}
	}
}
