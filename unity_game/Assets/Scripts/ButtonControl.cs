using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {
	
	public bool isPress;
public	Rigidbody c;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(IsPress){
			transform.localPosition=new Vector3(0,-6,0);
			c.useGravity=true;
		}
		else{
			transform.localPosition=new Vector3(0,0,0);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		IsPress=true;
	}
	
	void OnTriggerExit(Collider other) {
		IsPress=false;
	}
	
	public bool IsPress{
		get{return isPress;}
		set{
			isPress=value;
		}
	}
	
}
