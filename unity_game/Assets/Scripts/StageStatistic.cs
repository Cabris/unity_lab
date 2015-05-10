using UnityEngine;
using System.Collections;

public class StageStatistic : MonoBehaviour {

	//[SerializeField]
//	SwitchWall entrance,exit;
	Transform player;
	//[SerializeField]
	public float counter=0;
	//[SerializeField]
	Rect r=new Rect();
	// Use this for initialization
	void Start () {
		player=Extensions.Player.transform;
		Vector2 pos=new Vector2(transform.position.x, transform.position.z);
		//Vector2 pp=new Vector2(player.position.x, player.position.z);
		r.x=pos.x-25/2;
		r.y=pos.y-25/2;
		r.width=25;
		r.height=25;
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 pp=new Vector2(player.position.x, player.position.z);


		if(r.Contains(pp)){
			counter+=Time.deltaTime;
		}

	}
}























