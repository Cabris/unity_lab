using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BoneData
{
	Transform root;
	public List<Transform> transformsToSend;
	private Dictionary<Transform,BoneInfo> _bones;

	public BoneData (Transform t)
	{
		this.root=t;
		transformsToSend=new List<Transform>();
		_bones=new Dictionary<Transform,BoneInfo>();
	}
	
	public Hashtable GetHash(){
		Hashtable hash=toHash();
		hash.Add("cmd","b");
		return hash;
	}

	private Hashtable toHash(){
		Hashtable hash=new Hashtable();
		foreach(Transform boneTf in transformsToSend){
			Hashtable boneHash=new Hashtable();
			boneHash.Add("name",boneTf.name);
			boneHash.Add("bone",GetBoneInfo(boneTf));
			hash.Add(boneTf.name,boneHash);
			_bones[boneTf].localPos=boneTf.localPosition;
			_bones[boneTf].localRot=boneTf.localRotation;
		}
		return hash;
	}

	public bool IsSame(){
		bool r=true;
		if(_bones.Count<transformsToSend.Count){
			_bones.Clear();
			foreach(Transform boneTf in transformsToSend){
				BoneInfo bInfo=new BoneInfo();
				bInfo.localPos=boneTf.localPosition;
				bInfo.localRot=boneTf.localRotation;
				_bones.Add(boneTf,bInfo);
			}
			r=false;
		}else if(_bones.Count==transformsToSend.Count){
			foreach(Transform boneTf in transformsToSend){
				BoneInfo bInfo=_bones[boneTf];
				r=r&&bInfo.localPos==boneTf.localPosition&&bInfo.localRot==boneTf.localRotation;
			}
		}
		return r;
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

	class BoneInfo{
		public Vector3 localPos;
		public Quaternion localRot;
	}

}


