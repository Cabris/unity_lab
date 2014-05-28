using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class ClientController : MonoBehaviour {
	
	public GameObject prefabManagerPrefab;
	public PrefabManager prefabManager;
	// Use this for initialization
	void Start () {
		GameObject prefabManagerGo=Instantiate(prefabManagerPrefab) as GameObject;
		prefabManager=prefabManagerGo.GetComponent<PrefabManager>();
	}

	public void CreateSceneObject(SFSObject sdata){
		string type=SceneObject.GetType(sdata);
		string name=SceneObject.GetName(sdata);
		SFSObject t=SceneObject.GetTransform(sdata);
		GameObject sceneObj=prefabManager.InstantiateSceneObject(type);

		sceneObj.name=name;
		sceneObj.AddComponent<NetworkTransformReceiver>();

		NetworkTransformReceiver r=sceneObj.GetComponent<NetworkTransformReceiver>();
		r.StartReceiving();

		Vector3 pos=NetworkTransformReceiver.GetPos(t);
		Quaternion rot=NetworkTransformReceiver.GetRot(t);
		sceneObj.transform.position=pos;
		sceneObj.transform.rotation=rot;
		Rigidbody rigi=sceneObj.GetComponent<Rigidbody>();
		if(rigi!=null){
			rigi.isKinematic=true;
			rigi.useGravity=false;
		}
		//Debug.Log(sceneObj.name+" pos: "+pos+","+sceneObj.transform.position);
		r.ReceiveTransform(t);
		//NetworkTransformReceiver.SetTransform(sceneObj.transform,t);
		sceneObj.transform.parent=gameObject.transform;
	}






}
