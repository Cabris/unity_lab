using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage_1 : MonoBehaviour {

	[SerializeField]
	List <ButtonControl> buttons;
	[SerializeField]
	List <GameObject> cubes;
	[SerializeField]
	GameObject wall;
	private bool isFinished=false;
	Vector3 wallIniPos;
	[SerializeField]
	float g=11.815f;
	Dictionary<ButtonControl,bool> isButtonPress=new Dictionary<ButtonControl,bool>();
	// Use this for initialization
	void Start () {

		foreach(ButtonControl button in buttons){
			button.OnButtonPress+=ButtonPress;
			button.OnButtonUp+=ButtonUp;
			isButtonPress.Add(button,false);
		}
		wallIniPos=wall.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		isFinished=true;
		foreach(bool b in isButtonPress.Values){
			isFinished&=b;
		}
		if(isFinished){
			TweenPosition.Begin(wall,0.25f,new Vector3(0,-g,0)+wallIniPos);
		}
		else{
			TweenPosition.Begin(wall,0.25f,wallIniPos);
		}
		
	}
	
	void ButtonPress(ButtonControl control,GameObject hit){
		string buttonId=control.name.Split('-')[1];
		string[] d=hit.name.Split('-');
		string cubeId=d[d.Length-1];
		if(buttonId==cubeId){
			isButtonPress[control]=true;
		}
	}

	void ButtonUp(ButtonControl control,GameObject hit){
		string buttonId=control.name.Split('-')[1];
		string[] d=hit.name.Split('-');
		string cubeId=d[d.Length-1];
		if(buttonId==cubeId){
			isButtonPress[control]=false;
		}
	}
}
