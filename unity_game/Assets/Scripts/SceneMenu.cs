using UnityEngine;
using System.Collections;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class SceneMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SmartFoxClient	smartFoxClient =NetworkController.GetClient();
		if (smartFoxClient==null) {
			Application.LoadLevel("login");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
	
	}
}
