using UnityEngine;
using System.Collections;

public class DemoScene : MonoBehaviour {

	void OnGUI() {
		GUI.Label(new Rect(10, 10, 800, 20), "scan the qr code");
		GUI.Label(new Rect(10, 40, 800, 20), "press F1 to start receive orientation");
		GUI.Label(new Rect(10, 70, 800, 20), "press F2 to start transfer frame");
	}
}
