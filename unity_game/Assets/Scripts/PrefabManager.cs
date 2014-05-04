using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PrefabManager : MonoBehaviour {

	public List<SceneObject> prefabs=new List<SceneObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject InstantiateSceneObject(string type){
		GameObject g=findGoByType(type);
		if(g==null)
			return null;
		return Instantiate(g) as GameObject;
	}

	GameObject findGoByType(string type){
		foreach(SceneObject s in prefabs){
			if(s.type==type)
				return s.gameObject;
		}
		return null;
	}

}
