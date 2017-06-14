using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtrl1 : MonoBehaviour {

	public void onGoMenu2(string sceneName) {
		Application.LoadLevel (sceneName);
	}
}
