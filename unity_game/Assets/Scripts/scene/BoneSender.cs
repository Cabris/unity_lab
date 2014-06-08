using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using SmartFoxClientAPI.Data;
[RequireComponent(typeof(KinectModelControllerV2))]
public class BoneSender : MonoBehaviour {
	BoneData bd;
	KinectModelControllerV2 kinectModelController;
	public Transform target;
	// Use this for initialization
	void Start () {
		kinectModelController=GetComponent<KinectModelControllerV2>();
		bd=new BoneData(target);
		bd.transformsToSend.Add(kinectModelController.Head.transform);
		bd.transformsToSend.Add(kinectModelController.Shoulder_Left.transform);
		bd.transformsToSend.Add(kinectModelController.Shoulder_Right.transform);
		bd.transformsToSend.Add(kinectModelController.Elbow_Left.transform);
		bd.transformsToSend.Add(kinectModelController.Elbow_Right.transform);
		bd.transformsToSend.Add(kinectModelController.Wrist_Left.transform);
		bd.transformsToSend.Add(kinectModelController.Wrist_Right.transform);

		bd.transformsToSend.Add(kinectModelController.Hand_Left.transform);
		bd.transformsToSend.Add(kinectModelController.Hand_Right.transform);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		Hashtable hash=bd.GetHash();
//		hash.Add("object_name",name);
//		ClientNetworkController.SendExMsg("test","s",hash);
//		Debug.Log("send bones");
	}

	void LateUpdate() {

		Hashtable hash=bd.GetHash();
		hash.Add("object_name",name);
		ClientNetworkController.SendExMsg("test","s",hash);
		//Debug.Log("send bones");
	}

}
