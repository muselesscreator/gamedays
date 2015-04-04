using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButton : MonoBehaviour {

	public Sprite HomeSprite;
	public Sprite SoundOnSprite;
	public Sprite SoundOffSprite;
	public Sprite HurrySprite;
	public Sprite HurryingSprite;

	public Player Sammy;
	public Animator SammyAnim;

	private Image myImage;
	// Use this for initialization
	void Start () {
		Sammy = GameObject.Find("sammy").GetComponent<Player>();
		SammyAnim = GameObject.Find ("sammy").GetComponent<Animator>();
		myImage = transform.FindChild ("Image").GetComponent<Image>();
		if (myImage.sprite == SoundOnSprite && AudioListener.volume == 0) {
			myImage.sprite = SoundOffSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoHome ()
	{
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("panel")) {
			GameObject.Destroy (obj);
		}
		Application.LoadLevel ("StartScreen"); 
	}

	public void MuteToggleIcon () {
		if (myImage.sprite == SoundOnSprite) {
			AudioListener.volume = 0;
			myImage.sprite = SoundOffSprite;
		}
		else {
			AudioListener.volume = 1;
			myImage.sprite = SoundOnSprite;
		}
	}

	public void SpeedUp() {
		if (myImage.sprite == HurrySprite) {
			myImage.sprite = HurryingSprite;
			SammyAnim.SetBool("superSpeed", true);
			Sammy.speed = 6;
		}
		else {
			myImage.sprite = HurrySprite;
			SammyAnim.SetBool ("superSpeed", false);
			Sammy.speed = 3;
		}
	}
}
