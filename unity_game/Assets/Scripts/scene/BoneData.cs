using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class BoneData
{
	Transform root;
	public List<Transform> transformsToSend;
	public BoneData (Transform t)
	{
		this.root=t;
		transformsToSend=new List<Transform>();
		//findChildren(root,childrenT);
	}
	
	public Hashtable GetHash(){
		Hashtable hash=toHash();
		hash.Add("cmd","b");
		return hash;
	}

	private Hashtable toHash(){
		Hashtable hash=new Hashtable();
		foreach(Transform c in transformsToSend){
			Hashtable boneHash=new Hashtable();
			boneHash.Add("name",c.name);
			boneHash.Add("bone",GetBoneInfo(c));
			hash.Add(c.name,boneHash);
		}
		//hash=new Hashtable();
		return hash;
	}


//	private void findChildren(Transform t,List<Transform> tList){
//		for(int i=0;i<t.childCount;i++){
//			Transform c=t.GetChild(i);
////			Debug.Log(c.name);
//			tList.Add(c);
//			findChildren(c,tList);
//		}
//	}
	
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



}


