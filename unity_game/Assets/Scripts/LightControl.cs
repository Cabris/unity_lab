using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {
	[SerializeField]
	Light light;

	// Use this for initialization
	void Start () {
		light.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.collider.gameObject.tag=="Player")
			light.enabled=true;
	}
	
	void OnCollisionExit(Collision collision) {
		if(collision.collider.gameObject.tag=="Player")
			light.enabled=false;
	}
}
