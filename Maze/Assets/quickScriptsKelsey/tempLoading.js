#pragma strict
// NOTE: this script should be replaced before final release
// It is meant to simulate the game loading

var loadingBar : GameObject;
var amountLoaded : int;
var theSlider: UnityEngine.UI.Slider;

loadingBar = GameObject.Find ("LoadingBar");
theSlider = loadingBar.GetComponent(UnityEngine.UI.Slider);
amountLoaded = 0;

function Update () {
	if (amountLoaded == 100) {
 		Application.LoadLevel ("StartScreen"); 
	}
	amountLoaded++;
	theSlider.value = amountLoaded;
}