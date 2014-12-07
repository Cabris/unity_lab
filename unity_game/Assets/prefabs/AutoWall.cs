using UnityEngine;
using System.Collections;

public class AutoWall : MonoBehaviour {

	[SerializeField]
	ButtonControl button;
	[SerializeField]
	GameObject cube;
	[SerializeField]
	GameObject wall;
	private bool isFinished=false;
	Vector3 wallIniPos;
	[SerializeField]
	float g=12.49f;
	// Use this for initialization
	void Start () {
		button.OnButtonPress+=ButtonPress;
		button.OnButtonUp+=ButtonUp;
		wallIniPos=wall.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(isFinished){
			TweenPosition.Begin(wall,0.25f,new Vector3(0,-g,0)+wallIniPos);
		}
		else{
			TweenPosition.Begin(wall,0.25f,wallIniPos);
		}
		
	}
	
	void ButtonPress(ButtonControl control,GameObject hit){
		if(hit==cube){
			isFinished=true;
		}
	}
	
	void ButtonUp(ButtonControl control,GameObject hit){
		if(hit==cube){
			isFinished=false;
		}
	}
}
