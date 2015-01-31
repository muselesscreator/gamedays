using UnityEngine;
using System.Collections;

public class FishSpawner : MonoBehaviour {

	public GameObject myFish;
	Texture[] textures;
	public float lowY = -2f;
	public float highY = 1.2f;
	public float speed = 2f;

	// Use this for initialization
	void Start () {
		textures = GameObject.Find ("GameController").GetComponent<LevelController> ().textures;
	}

	public void addFish() {
		myFish = GameObject.Instantiate(Resources.Load<GameObject> ("Prefabs/Fish"), gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		myFish.name = "Fish";
		myFish.transform.parent = gameObject.transform;
		int lowY =  -100;
		int highY = 10;

		myFish.transform.localScale = Vector3.one * 0.7f;
		Texture texture = textures [Random.Range (1, textures.Length)];
		myFish.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = texture;
		myFish.GetComponent<Fish>().texture = texture;
	}

	public void removeFish() {
		Destroy (myFish);
		myFish = null;
		gameObject.tag = "EmptySpawner";
	}

	// Update is called once per frame
	void Update () {
	
	}
}
