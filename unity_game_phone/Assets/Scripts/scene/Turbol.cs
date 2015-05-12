using UnityEngine;
using System.Collections;

public class Turbol : MonoBehaviour {
	[SerializeField]
	OnTriggerDirectionalForce force;
	
	public float forceSpeed;
	[SerializeField]
	Renderer[] rs;
	// Use this for initialization
	void Start () {
		//rs=GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		force.ForceSpeed=forceSpeed;
		foreach(Renderer r in rs){
			Color cw=r.material.color;
			if(forceSpeed>0)
				cw.a=1;
			else
				cw.a=.2f;
			r.material.color=cw;
		}
	}
}
