using UnityEngine;
using System.Collections;

public class LaboratoryBalance : MonoBehaviour {
//	[SerializeField]
//	ButtonControl mass1Far,mass1Near,mass2Far,mass2Near;
//	[SerializeField]
//	Transform mass1,mass2;
	[SerializeField]
	Transform arm;
	float moveStep=0.1f;
	//public float Balance{get;private set;}
	public float Balance;
	
	// Use this for initialization
	void Start () {
		Balance=999;
//		mass1Far.OnButtonPress+=onButtonPress;
//		mass1Near.OnButtonPress+=onButtonPress;
//		mass2Far.OnButtonPress+=onButtonPress;
//		mass2Near.OnButtonPress+=onButtonPress;
	}
	
	// Update is called once per frame
	void Update () {
		Balance=arm.localRotation.eulerAngles.x;

	}

	void onButtonPress(ButtonControl bc){
//		if(bc==mass1Far)
//			mass1.Translate(new Vector3(-moveStep,0,0));
//		if(bc==mass1Near)
//			mass1.Translate(new Vector3(moveStep,0,0));
//		if(bc==mass2Far)
//			mass2.Translate(new Vector3(moveStep,0,0));
//		if(bc==mass2Near)
//			mass2.Translate(new Vector3(-moveStep,0,0));
	}
}
