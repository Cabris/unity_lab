using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class MechinePart : MonoBehaviour {
	Renderer r;
	public bool isPlaced;

	// Use this for initialization
	void Start () {
		collider.isTrigger=true;
		r=GetComponentInChildren<Renderer>();
		isPlaced=false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) {
		string n=other.name;
		if(n==name){
			Destroy(other.gameObject);
			isPlaced=true;
		}
	}
	
}
