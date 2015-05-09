
using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour {
	
	public GameObject Meteor;
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public GameObject[] spawners;
	
	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnMeteors");
	}
	
	IEnumerator SpawnMeteors() {
		yield return new WaitForSeconds(startWait);
		while (true) {
			GameObject myMeteor = Instantiate (Meteor) as GameObject;
			GameObject spawner = spawners[Random.Range (0, spawners.Length)];
			Debug.Log (spawner.name);
			myMeteor.transform.SetParent(spawner.transform, true);
			myMeteor.transform.localPosition = Vector3.zero;
			yield return new WaitForSeconds(spawnWait);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
