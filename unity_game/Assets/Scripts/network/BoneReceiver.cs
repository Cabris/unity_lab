using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using SmartFoxClientAPI.Data;
public class BoneReceiver : MonoBehaviour {
	public bool isScene;
	BoneData bd;
	public Transform target;
	public Transform[] transforms;
	SFSObject latestData;
	Queue q=new Queue();
	void Start () {
		bd=new BoneData(target);
		transforms=target.gameObject.GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(q.Count>0){
			SFSObject data=q.Dequeue() as SFSObject;
			SetBones(data);
			latestData=data;
		}
	}

	void LateUpdate() {
		if(latestData!=null)
			SetBones(latestData);
	}
	
	public void ReceiveBoneData(SFSObject data){
		q.Enqueue(data);
		//Debug.Log("ReceiveBoneData");
	}
	
	public void SetBones (SFSObject data)
	{
		//Debug.Log("SetBones");
		foreach(object k in data.Keys()){
			string keyname=k.ToString();
			//Debug.Log("keyname: "+keyname);
			foreach(Transform c in transforms){
				string n=c.gameObject.name;
				if(n==keyname){
					SFSObject boneInfo=data.GetObj(keyname) as SFSObject;
					SetTansform(boneInfo,c);
					break;
				}
			}
		}
	}

	private void SetTansform(SFSObject boneInfo,Transform t){
		Vector3 p=new Vector3();
		SFSObject boneTransform=boneInfo.GetObj("bone") as SFSObject;
		p.x=(float)boneTransform.GetNumber("x");
		p.y=(float)boneTransform.GetNumber("y");
		p.z=(float)boneTransform.GetNumber("z");
		Quaternion r=new Quaternion();
		r.x=(float)boneTransform.GetNumber("rx");
		r.y=(float)boneTransform.GetNumber("ry");
		r.z=(float)boneTransform.GetNumber("rz");
		r.w=(float)boneTransform.GetNumber("rw");
	//	Debug.Log("SetTansform: "+boneInfo.GetString("name"));
		t.localPosition=p;
		t.localRotation=r;
	}
	
}
