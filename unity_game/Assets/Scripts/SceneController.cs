using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class SceneController : MonoBehaviour {

	public List<GameObject> sceneObjs=new List<GameObject>();
	public GameObject prefabManagerPrefab;

	// Use this for initialization
	void Start () {
		SceneObject[] s=GetComponentsInChildren<SceneObject>();
		foreach(SceneObject ss in s){
			GameObject g=ss.gameObject;
			sceneObjs.Add(g);
			string type=ss.SceneObjType;
			g.AddComponent<NetworkTransformSender>();
			NetworkTransformSender sender=g.GetComponent<NetworkTransformSender>();
			sender.StartSending();
			Debug.Log(g.name+", "+type+".");
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
