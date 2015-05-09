using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButton : MonoBehaviour {

	public Sprite HomeSprite;
	public Sprite SoundOnSprite;
	public Sprite SoundOffSprite;
	public Sprite HurrySprite;
	public Sprite HurryingSprite;
	public Sprite ToVertical;
	public Sprite FromVertical;

	public Player Sammy;
	public Animator SammyAnim;
	public GameObject MyCamera;
	public GameObject VertCamera;
	public GameObject OffVertCamera;
	public Board theBoard;

	private Image myImage;
	// Use this for initialization
	void Start () {
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
		Sammy = GameObject.Find("sammy").GetComponent<Player>();
		SammyAnim = GameObject.Find ("sammy").GetComponent<Animator>();
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

	public void GoToVertical() {
		MyCamera = GameObject.Find ("Main Camera");
		VertCamera = GameObject.Find ("VertCamera");
		OffVertCamera = GameObject.Find ("OffVertCamera");
		theBoard = GameObject.Find ("GameController").GetComponent<Board> ();

		if (myImage.sprite == ToVertical) {
			myImage.sprite = FromVertical;
			theBoard.isVertical = true;
			MyCamera.transform.parent = VertCamera.transform;
			foreach (GameObject sign in theBoard.signs) {
				sign.SetActive(false);
			}
		}
		else {
			myImage.sprite = ToVertical;
			theBoard.isVertical = false;
			MyCamera.transform.parent = OffVertCamera.transform;
			foreach (GameObject sign in theBoard.signs) {
				sign.SetActive(true);
			}
		}
		MyCamera.transform.localPosition = Vector3.zero;
		MyCamera.transform.localRotation = Quaternion.identity;
	}

}
