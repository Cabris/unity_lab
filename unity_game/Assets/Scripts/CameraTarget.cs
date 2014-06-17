using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour {
	public WiiController wii;
	bool isAlreadyBack;
	// Use this for initialization
	void Start () {
		isAlreadyBack=false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isAlreadyBack&&wii.isPressA){
			transform.Rotate(new Vector3(0,180,0));
			isAlreadyBack=true;
		}
		if(!wii.isPressA&&isAlreadyBack){
			transform.Rotate(new Vector3(0,-180,0));
			isAlreadyBack=false;
		}
	}
}
