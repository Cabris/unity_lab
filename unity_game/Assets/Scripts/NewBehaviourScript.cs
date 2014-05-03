using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		NetworkTransformReceiver r=GetComponent<NetworkTransformReceiver>();
		GUI.Label(new Rect(10, Screen.height-85, 200, 24), "qc: " + r.qc);
	}

}
