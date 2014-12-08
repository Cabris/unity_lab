using UnityEngine;
using System.Collections;

public class SwitchWall : MonoBehaviour {

	[SerializeField]
	GameObject wall;
	[SerializeField]
	float g=12.49f;
	[SerializeField]
	float duration=.25f;
	Vector3 wallIniPos;
	public bool Open=false;
	// Use this for initialization
	void Start () {
		wallIniPos=wall.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(Open){
			TweenPosition.Begin(wall,duration,new Vector3(0,-g,0)+wallIniPos);
		}
		else{
			TweenPosition.Begin(wall,duration,wallIniPos);
		}
	}
}
