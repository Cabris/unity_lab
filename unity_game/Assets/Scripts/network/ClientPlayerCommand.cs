using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientPlayerCommand : MonoBehaviour
{
	public bool diff=false;
	public bool isWalking=false;
	public bool isTurnRight=false;
	public bool isTurnLeft=false;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{ 
		diff=false;
		if (Input.GetKeyDown (KeyCode.W)) {
			diff=true;	
			isWalking=true;
		} 
		if (Input.GetKeyUp (KeyCode.W)) {
			diff=true;	
			isWalking=false;
		}
		if (Input.GetKeyDown (KeyCode.A)) {//left
			diff=true;	
			isTurnLeft=true;
		} 
		if (Input.GetKeyDown (KeyCode.D)) {//right
			diff=true;	
			isTurnRight=true;
		}
		if (Input.GetKeyUp (KeyCode.A)) {//left
			diff=true;	
			isTurnLeft=false;
		} 
		if (Input.GetKeyUp (KeyCode.D)) {//right
			diff=true;	
			isTurnRight=false;
		}
		
		SmartFoxClient client = ClientNetworkController.GetClient ();
		
		SFSObject data = new SFSObject ();
		data.Put ("cmd", "m");  //We put _cmd = "t" here to know that this object contains transform sync data. 
		data.Put ("isWalking", isWalking);
		data.Put ("isTurnLeft", isTurnLeft);
		data.Put ("isTurnRight", isTurnRight);
		data.Put ("object_name", this.name);
		
		if(diff){
			client.SendObject (data);
		}
		
		
	}
	
	
}
