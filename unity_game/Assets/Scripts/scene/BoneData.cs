using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class BoneData
{
	Transform root;
	List<Transform> childrenT;
	public BoneData (Transform t)
	{
		this.root=t;
		childrenT=new List<Transform>();
		findChildren(root,childrenT);
	}
	
	public void SetBones (SFSObject data)
	{
		foreach(Transform c in childrenT){
			string n=c.name;
			Debug.Log("SetBones "+n);
			SFSObject boneInfo=data.GetObj(n) as SFSObject;
			SetTansform(boneInfo,c);
		}
	}
	
	public Hashtable GetHash(){
		Hashtable hash=toHash();
		hash.Add("cmd","b");
		return hash;
	}

	private Hashtable toHash(){
		Hashtable hash=new Hashtable();
		foreach(Transform c in childrenT){
			Hashtable boneHash=new Hashtable();
			boneHash.Add("name",c.name);
			boneHash.Add("bone",GetBoneInfo(c));
			hash.Add(c.name,boneHash);
		}
		return hash;
	}


	private void findChildren(Transform t,List<Transform> tList){
		for(int i=0;i<t.childCount;i++){
			Transform c=t.GetChild(i);
//			Debug.Log(c.name);
			tList.Add(c);
			findChildren(c,tList);
		}
	}
	
	private Hashtable GetBoneInfo(Transform bt){
		Hashtable h=new Hashtable();
		h.Add("x",bt.localPosition.x);
		h.Add("y",bt.localPosition.y);
		h.Add("z",bt.localPosition.z);
		h.Add("rx",bt.localRotation.x);
		h.Add("ry",bt.localRotation.y);
		h.Add("rz",bt.localRotation.z);
		h.Add("rw",bt.localRotation.w);
		return h;
	}

	private void SetTansform(SFSObject boneInfo,Transform t){
		Vector3 p=new Vector3();
		p.x=(float)boneInfo.GetNumber("x");
		p.y=(float)boneInfo.GetNumber("y");
		p.z=(float)boneInfo.GetNumber("z");
		Quaternion r=new Quaternion();
		r.x=(float)boneInfo.GetNumber("rx");
		r.y=(float)boneInfo.GetNumber("ry");
		r.z=(float)boneInfo.GetNumber("rz");
		r.w=(float)boneInfo.GetNumber("rw");
	}

}


