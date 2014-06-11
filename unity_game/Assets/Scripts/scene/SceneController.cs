using UnityEngine;
//using UnityEditor;
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
		string path = string.Format("file://{0}/_AssetBundles/{1}.assetBundles" ,  "c:" , assetName);
		WWW bundle = new WWW(path);
		yield return bundle;
		GameObject scene=UnityEngine.Object.Instantiate(bundle.assetBundle.mainAsset) as GameObject;
		Debug.Log("loaded: "+path);
		yield return scene;

		SceneObject[] s=scene.GetComponentsInChildren<SceneObject>();
		foreach(SceneObject ss in s){
			GameObject g=ss.gameObject;
			g.transform.parent=transform;
			sceneObjs.Add(g);
			g.AddComponent<NetworkTransformSender>();
			NetworkTransformSender sender=g.GetComponent<NetworkTransformSender>();
			sender.StartSending();
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
