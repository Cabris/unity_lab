using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class SceneController : MonoBehaviour {

	public List<GameObject> sceneObjs=new List<GameObject>();
	
	// Use this for initialization
	void Start () {
		NetworkTransformSender[] s=GetComponentsInChildren<NetworkTransformSender>();
		foreach(NetworkTransformSender ss in s){
			GameObject g=ss.gameObject;
			sceneObjs.Add(g);
			GameObject p=PrefabUtility.GetPrefabParent(g) as GameObject;
			string path=AssetDatabase.GetAssetPath(p);
			Debug.Log(g.name+", "+path+".");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
