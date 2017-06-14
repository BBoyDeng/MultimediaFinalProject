using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtrl2 : MonoBehaviour {

	public void onStartClassicalGame3X3(string sceneName) {
		Application.LoadLevel (sceneName);
	}

	public void onStartClassicalGame4X4(string sceneName) {
		Application.LoadLevel (sceneName);
	}

	public void onStartClassicalGame3X3X3(string sceneName) {
		Application.LoadLevel (sceneName);
	}

	public void onStartTimeGame3X3(string sceneName) {
		Application.LoadLevel (sceneName);
	}

	public void onStartTimeGame4X4(string sceneName) {
		Application.LoadLevel (sceneName);
	}

	public void onStartTimeGame3X3X3(string sceneName) {
		Application.LoadLevel (sceneName);
	}
}
