using UnityEngine;
using System.Collections.Generic;

public class SceneLogic : MonoBehaviour {
	[SerializeField]
	List<Transform> stages;
	PlayerBeheaver player;
	[SerializeField]
	UILabel label;

	// Use this for initialization
	void Start () {
		player=Extensions.Player;

	}
	
	// Update is called once per frame
	void Update () {

		string msg="Welcome! ";
		msg+=("[FFFF00]"+player.playerId+"[-].\n");
		msg+=("You have been chosen to handle the tests of several Physic phenomenon.\n" +
		      "Before you start your journey you need to get used abnout how to manipulate the game.\n");

		
		
		label.text=msg;
//		foreach(Transform t in stages){
//			if(Vector3.Distance(player.position,t.position)>30)
//				t.gameObject.SetActive(false);
//			else
//				t.gameObject.SetActive(true);
//		}
	}
}
