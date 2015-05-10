using UnityEngine;
using System.Collections;

public class Stage_Time : MonoBehaviour {
	//[SerializeField]
	//PauseRigidBody pause;
	[SerializeField]
	ButtonControl button;
	[SerializeField]
	SwitchWall wall;
	// Use this for initialization
	void Start () {
		//button.OnButtonPress+=OnButtonPress;
		//button.OnButtonUp+=OnButtonUp;
	}
	
	// Update is called once per frame
	void Update () {
		wall.Open=button.IsPress;
	}

	void OnButtonPress(ButtonControl control,GameObject hit){
		//if(hit.tag=="Player")
			//pause.pause=true;
	}

	void OnButtonUp(ButtonControl control,GameObject hit){
		//if(hit.tag=="Player")
			//pause.pause=false;
	}

}
