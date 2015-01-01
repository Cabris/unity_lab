using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SwitchWall))]
public class AutoWall : MonoBehaviour {
	SwitchWall wall;
	// Use this for initialization
	void Start () {
		wall=GetComponent<SwitchWall>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag=="Player")
			wall.Open=true;
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag=="Player")
			wall.Open=false;
	}
}
