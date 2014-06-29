using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartFoxClientAPI.Data;
using System;

public class ClientController : MonoBehaviour {
	public static string AssetName{get;set;} 
	private bool isLoaded=false;
	List<SFSObject> sceneObjectDatas;
	// Use this for initialization
	void Start () {
		sceneObjectDatas=new List<SFSObject>();
		if(AssetName!=null&&AssetName.Length>0){
			Coroutine co= StartCoroutine(LoadGameObject(AssetName));
		}
	}
	
	private IEnumerator LoadGameObject(string assetName){
		WWW bundle=null;
		string path = string.Format(Extensions.AssetBundleLoaction, assetName);
		Debug.Log("p:"+path);
		try{
			//loading assetbundle
			bundle = new WWW(path);
		}
		catch(Exception e){
			Debug.LogException(e);
		}
			//wait for loaded
			yield return bundle;
			
			//Instantiate GameObject wait
			GameObject scene=UnityEngine.Object.Instantiate(bundle.assetBundle.mainAsset) as GameObject;
			Debug.Log("loaded: "+path);
			yield return scene;
			
			SceneObject[] s=scene.GetComponentsInChildren<SceneObject>();
			foreach(SceneObject ss in s){
				GameObject g=ss.gameObject;
				g.transform.parent=transform;
			}
			Debug.Log("client loaded from asset: "+AssetName);
			Destroy(scene);
			isLoaded=true;
			bundle.assetBundle.Unload(false);
		

	}
	
	public void HandleSceneObject(SFSObject sdata){
		if(!sceneObjectDatas.Contains(sdata))
			sceneObjectDatas.Add(sdata);
	}

	private void initSceneObject(SFSObject sdata){
		string name=SceneObject.GetName(sdata);
		SFSObject t=SceneObject.GetTransform(sdata);
		GameObject sceneObj=GameObject.Find(name);	
		sceneObj.name=name;

//		sceneObj.AddComponent<NetworkTransformReceiver>();
//		NetworkTransformReceiver ntReceiver=sceneObj.GetComponent<NetworkTransformReceiver>();
//		ntReceiver.StartReceiving();
//		ntReceiver.ReceiveTransform(t);

		sceneObj.AddComponent<SimpleTransformSync>();

		Vector3 pos=NetworkTransformReceiver.GetPos(t);
		Quaternion rot=NetworkTransformReceiver.GetRot(t);
		Vector3 sca=NetworkTransformReceiver.GetScale(t);
		sceneObj.transform.position=pos;
		sceneObj.transform.rotation=rot;
		Rigidbody rig=sceneObj.rigidbody;
		if(rig!=null){
			rig.isKinematic=true;
			rig.useGravity=false;
		}


	}

	void Update () {
		if(isLoaded){
			List<SFSObject> re=new List<SFSObject>();
			foreach(SFSObject s in sceneObjectDatas){
				initSceneObject(s);
				re.Add(s);
			}
			foreach(SFSObject r in re){
				sceneObjectDatas.Remove(r);
			}
		}
	}
	
}
