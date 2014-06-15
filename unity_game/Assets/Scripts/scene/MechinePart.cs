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
		r.enabled=isPlaced;
		if(rigidbody!=null)
			rigidbody.isKinematic=!isPlaced;
	}
	
	void OnTriggerEnter(Collider other) {
		string n=other.name;
		if(n==name){
			Destroy(other.gameObject,1.5f);
			isPlaced=true;
		}
	}
	
}
