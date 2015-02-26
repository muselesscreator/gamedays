#pragma strict
 var soundEnabled : boolean;
 
var soundOnImage : GameObject;
var soundOffImage : GameObject;


soundEnabled = soundOnImage.activeSelf;

soundOnImage.SetActive (soundEnabled);
soundOffImage.SetActive (!soundEnabled);
 
 
function MuteToggleIcon () {
	soundEnabled = soundOnImage.activeSelf;
 	soundOnImage.SetActive (!soundEnabled);
 	soundOffImage.SetActive (soundEnabled);
 }