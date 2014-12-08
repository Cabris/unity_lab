using UnityEngine;
using System.Collections;

public class ButtonWall : MonoBehaviour {

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
	[SerializeField]
	float duration=.25f;
	// Use this for initialization
	void Start () {
		button.OnButtonPress+=ButtonPress;
		button.OnButtonUp+=ButtonUp;
		wallIniPos=wall.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(isFinished){
			TweenPosition.Begin(wall,duration,new Vector3(0,-g,0)+wallIniPos);
		}
		else{
			TweenPosition.Begin(wall,duration,wallIniPos);
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
