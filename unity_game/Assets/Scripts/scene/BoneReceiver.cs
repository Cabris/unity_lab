using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using SmartFoxClientAPI.Data;
public class BoneReceiver : MonoBehaviour {
	public bool isScene;
	BoneData bd;
	// Use this for initialization
	void Start () {
		bd=new BoneData(transform);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	public void ReceiveBoneData(SFSObject data){
		bd.SetBones(data);
		Debug.Log("ReceiveBoneData");
	}
	


}
