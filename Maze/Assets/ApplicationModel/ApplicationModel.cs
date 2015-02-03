using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplicationModel : MonoBehaviour {
	public static bool isPuzzle = false;

	public static BoardTemplate template;

	public static void setIsPuzzle (bool puz) {
		isPuzzle = puz;
	}

	public static void setTemplate (BoardTemplate bt) {
		template = bt;
	}

}
