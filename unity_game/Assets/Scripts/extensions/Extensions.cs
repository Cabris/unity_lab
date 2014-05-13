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
}


