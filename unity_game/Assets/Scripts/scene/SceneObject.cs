using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class SceneObject : MonoBehaviour {
	[SerializeField]
	public string type;
	public string sceneObjName;
	public Transform tra;

	private string _destory="d";


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
	
	public string SceneObjType{
		get {return type;}
	}

	public void DoDestoryAndSend (){
		GameObject.Destroy (gameObject);
		Hashtable d = new Hashtable ();
		d.Add ("msg",_destory);
	}

	private void SendMessage(Hashtable data){
		data.Add ("object_name",name);
		data.Add ("cmd","sceneObject");
		NetworkController.SendExMsg ("test","b",data);
	}

	public void ReceiveMessage(SFSObject data){
		string msg = data.GetString ("msg");
		if(msg==_destory)
			GameObject.Destroy (gameObject);
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
