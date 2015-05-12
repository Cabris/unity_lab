using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class MechinePart : MonoBehaviour {
	Renderer r;
	public bool isPlaced;
	[SerializeField]
	GameObject otherPart;
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
			if(other.gameObject==otherPart){
					GameObject.Destroy (otherPart);
				isPlaced=true;

			}
	}
	
}
