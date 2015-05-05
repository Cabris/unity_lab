using UnityEngine;
using System.Collections.Generic;

public class SceneLogic : MonoBehaviour {
	[SerializeField]
	List<Transform> stages;
	Transform player;
	// Use this for initialization
	void Start () {
		player=GameObject.Find("SceneLogic/local_player_standard").transform;
		GameObject stgs=GameObject.Find("SceneLogic/stages");
		foreach(Transform t in stgs.transform){
			stages.Add(t);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		foreach(Transform t in stages){
//			if(Vector3.Distance(player.position,t.position)>30)
//				t.gameObject.SetActive(false);
//			else
//				t.gameObject.SetActive(true);
//		}
	}
}
