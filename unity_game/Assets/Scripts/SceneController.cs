using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class SceneController : MonoBehaviour {

	public List<GameObject> sceneObjs=new List<GameObject>();
	public GameObject prefabManagerPrefab;
	Camera camera;
	Vector3 cameraIniPos;
	Quaternion cameraIniRot;
	// Use this for initialization
	void Start () {
		SceneObject[] s=GetComponentsInChildren<SceneObject>();
		foreach(SceneObject ss in s){
			GameObject g=ss.gameObject;
			sceneObjs.Add(g);
//			string type=ss.SceneObjType;
			g.AddComponent<NetworkTransformSender>();
			NetworkTransformSender sender=g.GetComponent<NetworkTransformSender>();
			sender.StartSending();
//			Debug.Log(g.name+", "+type+".");
		}
		camera=Camera.main;
		cameraIniPos=camera.transform.position;
		cameraIniRot=camera.transform.rotation;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void FocusTo(Transform t){
		SmoothFollow f=(SmoothFollow)camera.GetComponent("SmoothFollow");
		f.target=t;
		if(t==null){
			camera.transform.position=cameraIniPos;
			camera.transform.rotation=cameraIniRot;
		}
	}
}
