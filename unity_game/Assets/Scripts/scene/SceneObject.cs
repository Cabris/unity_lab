using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class SceneObject : NetworkObject {
	[SerializeField]
	public string type;
	public string sceneObjName;
	public Transform tra;

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
	
	public string SceneObjType{
		get {return type;}
	}

	public void ForceClientMove (){
		NetworkTransformSender sender=GetComponent<NetworkTransformSender>();
		if(sender!=null){
			sender.ForceSendTransform();
		}
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

}
