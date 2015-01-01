using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SwitchWall))]
public class ButtonWall : MonoBehaviour {

	[SerializeField]
	ButtonControl button;
	[SerializeField]
	GameObject cube;
	[SerializeField]
	SwitchWall wall;
	private bool isFinished=false;
	// Use this for initialization
	void Start () {
		button.OnButtonPress+=ButtonPress;
		button.OnButtonUp+=ButtonUp;
	}
	
	// Update is called once per frame
	void Update () {
		if(cube==null)
			isFinished=button.IsPress;
		wall.Open=isFinished;
	}
	
	void ButtonPress(ButtonControl control,GameObject hit){
		if(cube!=null&&hit==cube){
			isFinished=true;
		}
	}
	
	void ButtonUp(ButtonControl control,GameObject hit){
		if(cube!=null&&hit==cube){
			isFinished=false;
		}
	}
}
