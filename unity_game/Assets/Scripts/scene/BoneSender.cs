using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using SmartFoxClientAPI.Data;
public class BoneSender : MonoBehaviour {
	BoneData bd;
	public GameObject master;
	// Use this for initialization
	void Start () {
		bd=new BoneData(transform);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Hashtable hash=bd.GetHash();
		hash.Add("object_name",master.name);
		ClientNetworkController.SendExMsg("test","s",hash);
		//Debug.Log("send bones");
	}


}
