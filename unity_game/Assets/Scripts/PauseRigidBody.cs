using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseRigidBody : MonoBehaviour {
	[SerializeField]
	GameObject target;
	//[SerializeField]
	Rigidbody[] bodys;
	//[SerializeField]
	public bool pause=false;
	bool isPaused=false;
	Dictionary<Rigidbody,Vector3> velocities=new Dictionary<Rigidbody, Vector3>();
	Dictionary<Rigidbody,Vector3> angularVelocities=new Dictionary<Rigidbody, Vector3>();

	// Use this for initialization
	void Start () {
		bodys=target.GetComponentsInChildren<Rigidbody>();
		foreach(Rigidbody r in bodys){
			velocities.Add(r,Vector3.zero);
			angularVelocities.Add(r,Vector3.zero);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(pause&&!isPaused){
			foreach (Rigidbody r in bodys){
				velocities[r]=r.velocity;
				angularVelocities[r]=r.angularVelocity;
				r.velocity = Vector3.zero;
				r.angularVelocity=Vector3.zero;
				r.useGravity = false;
				r.isKinematic = true;
			}
			isPaused=pause;
		}
		if(!pause&&isPaused){
			foreach (Rigidbody r in bodys){
				r.useGravity = true;
				r.isKinematic = false;
				r.velocity = velocities[r];
				r.angularVelocity=angularVelocities[r];
			}
			isPaused=pause;
		}
	}
}
