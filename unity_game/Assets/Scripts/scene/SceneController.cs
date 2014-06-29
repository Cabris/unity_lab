using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {
	public static string AssetName{get;set;} 
	public List<GameObject> sceneObjs=new List<GameObject>();
	private bool isLoaded=false;
	Camera _camera;
	Vector3 cameraIniPos;
	Quaternion cameraIniRot;
	// Use this for initialization
	void Start () {
		
		if(AssetName!=null&&AssetName.Length>0){
			Coroutine co= StartCoroutine(LoadGameObject(AssetName));
		}
		_camera=Camera.main;
		cameraIniPos=_camera.transform.position;
		cameraIniRot=_camera.transform.rotation;
	}
	
	
	private IEnumerator LoadGameObject(string assetName){
		WWW bundle=null;
		string path = string.Format(Extensions.AssetBundleLoaction, assetName);
		Debug.Log("p:"+path);
		try{
		    bundle = new WWW(path);
		}
		catch(Exception e){
			Debug.LogException(e);
		}
		yield return bundle;
		GameObject scene=UnityEngine.Object.Instantiate(bundle.assetBundle.mainAsset) as GameObject;
		Debug.Log("loaded: "+path);

		yield return scene;
		
		SceneObject[] s=scene.GetComponentsInChildren<SceneObject>();
		foreach(SceneObject ss in s){
			GameObject sceneObj=ss.gameObject;
			sceneObj.transform.parent=transform;
			sceneObjs.Add(sceneObj);

//			sceneObj.AddComponent<NetworkTransformSender>();
//			NetworkTransformSender sender=sceneObj.GetComponent<NetworkTransformSender>();
//			sender.StartSending();

			sceneObj.AddComponent<SimpleTransformSync>();
		
		}
		Destroy(scene);
		Debug.Log("scene loaded from asset: "+AssetName);
		isLoaded=true;
		bundle.assetBundle.Unload(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void FocusTo(Transform t){
		SmoothFollow f=(SmoothFollow)_camera.GetComponent("SmoothFollow");
		f.target=t;
		if(t==null){
			_camera.transform.position=cameraIniPos;
			_camera.transform.rotation=cameraIniRot;
		}
	}
}
