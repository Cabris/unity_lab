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
	Queue q=new Queue();
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
	
	void Update () {
		if(q.Count>0){
			Hashtable data=q.Dequeue() as Hashtable;
			ClientNetworkController.SendExMsg("test","s",data);
		}
	}
	
	void LateUpdate() {
		if(!bd.IsSame()){
			Hashtable data=bd.GetHash();
			data.Add("object_name",name);
			q.Enqueue(data);
		}
	}
	
}
