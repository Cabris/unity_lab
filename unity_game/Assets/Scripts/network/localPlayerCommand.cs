using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class localPlayerCommand : MonoBehaviour
{
	public bool diff=false;
	public bool isWalking=false;
	public string key="";
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{ 
		isWalking=false;
		key="null";
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
			key="left";
		} else if (Input.GetKeyDown (KeyCode.S)) {//back
			diff=true;	
			key="back";
		} else if (Input.GetKeyDown (KeyCode.D)) {//right
			diff=true;	
			key="right";
		}
		
		SmartFoxClient client = ClientNetworkController.GetClient ();
		
		SFSObject data = new SFSObject ();
		data.Put ("_cmd", "m");  //We put _cmd = "t" here to know that this object contains transform sync data. 
		data.Put ("isWalking", isWalking);
		data.Put ("key", key);
		data.Put ("object_name", this.name);
		
		if(diff)
			client.SendObject (data);
		
		
	}
	
	
}
