using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
using System;

public class PlayerStatus : MonoBehaviour {

	public SkinnedMeshRenderer smr;
	public Color color;
	public string userName;
	// Use this for initialization
	void Start () {
		smr=GetComponentInChildren<SkinnedMeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		smr.material.color=color;
	}

	public Hashtable GetAsHashtable(){
		return GetAsSFSObject().ToHashTable();
	}

	public SFSObject GetAsSFSObject(){
		SFSObject data=new SFSObject();
		data.Put("cmd","playerStatus");
		data.Put("colorR",this.color.r);
		data.Put("colorG",this.color.g);
		data.Put("colorB",this.color.b);
		data.Put("colorA",this.color.a);
		data.Put("name",this.gameObject.name);
		data.Put("userName",this.userName);
		Hashtable t=NetworkTransform.GetTransformAsHash(this.transform);
		data.PutDictionary("transform",t);
		return data;
	}

	public void FromHashtable(SFSObject data){
		color.r=Convert.ToSingle(data.GetNumber("colorR"));
		color.g=Convert.ToSingle(data.GetNumber("colorG"));
		color.b=Convert.ToSingle(data.GetNumber("colorB"));
		color.a=Convert.ToSingle(data.GetNumber("colorA"));
		this.gameObject.name=data.GetString("name");
		this.userName=data.GetString("userName");
		SFSObject t=data.GetObj("transform");
		NetworkTransformReceiver.SetTransform(transform,t);
	} 

}
