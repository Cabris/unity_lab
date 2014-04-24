using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class serverPlayerCommand : MonoBehaviour {

	public bool isWalking;
	public string key;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ReceiveCommand(SFSObject data) {
		PlayerInput input=GetComponent<PlayerInput>();
		isWalking=data.GetBool("isWalking");
		key=data.GetString("key");
		
		if(input!=null){
			if(isWalking){
				input.StartWalk();
			}
			else{
				input.EndWork();
			}
//			Debug.Log("input: "+data);
			if(key=="left")
				input.Left();
			if(key=="right")
				input.Right();
			if(key=="back")
				input.Back();
		}
	}
}
