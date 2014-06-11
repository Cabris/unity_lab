using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class SceneObject : MonoBehaviour {
	[SerializeField]
	public string type;
	public string sceneObjName;
	public Transform tra;

	// Use this for initialization
	void Start () {
		sceneObjName=gameObject.name;
		tra=gameObject.transform;

	}

	public SFSObject GetDataAsSfs(){
		SFSObject data=new SFSObject();
		data.Put("name",sceneObjName);
		data.Put("type",type);
	 
		SFSObject tf=NetworkTransform.GetTransformAsSfs(tra);
		data.Put("transform",tf);
		return data;
	}

	public static string GetType(SFSObject data){
		return data.GetString("type");
	}

	public static  string GetName(SFSObject data){
		return data.GetString("name");
	}

	public static  SFSObject GetTransform(SFSObject data){
		return data.GetObj("transform") as SFSObject;
	}

	public string SceneObjType{
		get {return type;}
	}
	
}
