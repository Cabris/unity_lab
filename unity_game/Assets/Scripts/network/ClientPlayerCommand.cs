using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientPlayerCommand : MonoBehaviour
{
	public bool isWalking=false;
	public bool isTurnRight=false;
	public bool isTurnLeft=false;
	
	bool isWalkingLast=false;
	bool isTurnRightLast=false;
	bool isTurnLeftLast=false;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	void FixedUpdate ()
	{ 
		isWalking=false;
		isTurnRight=false;
		isTurnLeft=false;
		if (Input.GetKey (KeyCode.W)) {	
			isWalking=true;
		} 
		if (Input.GetKey (KeyCode.A)) {//left
			isTurnLeft=true;
		} 
		if (Input.GetKey (KeyCode.D)) {//right	
			isTurnRight=true;
		}
		
		if(isTurnLeft!=isTurnLeftLast||isTurnRight!=isTurnRightLast||isWalking!=isWalkingLast){
			isWalkingLast=isWalking;
			isTurnLeftLast=isTurnLeft;
			isTurnRightLast=isTurnRight;
			SmartFoxClient client = ClientNetworkController.GetClient ();
			
			Hashtable data=new Hashtable();
			data.Add("cmd", "m");
			data.Add("isWalking", isWalking);
			data.Add("isTurnLeft", isTurnLeft);
			data.Add("isTurnRight", isTurnRight);
			data.Add("object_name", this.name);
			client.SendXtMessage("test","s",data);
		}
		
	}
	
	
}
