using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage_Button_cube : MonoBehaviour {

	private bool isFinished=false;
	[SerializeField]
	List <ButtonControl> buttons;
	[SerializeField]
	List <GameObject> cubes;
	Dictionary<ButtonControl,bool> isButtonPress=new Dictionary<ButtonControl,bool>();
	[SerializeField]
	SwitchWall swichWall;
	// Use this for initialization
	void Start () {
		foreach(ButtonControl button in buttons){
			button.OnButtonPress+=ButtonPress;
			button.OnButtonUp+=ButtonUp;
			isButtonPress.Add(button,false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		isFinished=true;
		foreach(bool b in isButtonPress.Values){
			isFinished&=b;
		}
		swichWall.Open=isFinished;
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
