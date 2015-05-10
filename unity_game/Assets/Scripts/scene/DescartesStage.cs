using UnityEngine;
using System.Collections;

public class DescartesStage : MonoBehaviour {

	[SerializeField]
	DescartesCube[] cubes;
	//[SerializeField]
	//Transform temp;
	[SerializeField]
	SwitchWall wall;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		bool isfinished=true;
		foreach(DescartesCube c in cubes){
			Vector2 pos=new Vector2(Extensions.toInt(c.transform.localPosition.x),
			                        Extensions.toInt(c.transform.localPosition.z));
			isfinished&=(c.targetPosition==pos);
		}
		//temp.localPosition=new Vector3(pos.x,temp.localPosition.y,pos.y);
		wall.Open=isfinished;
	}




}
