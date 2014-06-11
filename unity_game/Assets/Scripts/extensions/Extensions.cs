using System;
using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;

public static class Extensions
{
	public static Hashtable ToHashTable(this SFSObject s){
		Hashtable data=new Hashtable();
		foreach(object k in s.Keys()){
			object value=s.Get(k);
			data.Add(k,value);
		}
		return data;
	}

	public static SFSObject ToSFSObject(this Hashtable h){
		SFSObject data=new SFSObject();
		foreach(object k in h.Keys){
			object value=h[k];
			data.Put(k,value);
		}
		return data;
	}


}


