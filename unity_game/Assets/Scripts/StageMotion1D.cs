using UnityEngine;
using System.Collections;

public class StageMotion1D : MonoBehaviour {
	[SerializeField]
	ButtonControl iniB,fireB,endB;
	[SerializeField]
	float power=30;
	[SerializeField]
	Turbol t;
	[SerializeField]
	Grapher1 grapher;
	//[SerializeField]
	//Transform cube;
	[SerializeField]
	GrapherWall gWall;
	// Use this for initialization
	void Start () {
		fireB.OnButtonPress+=OnButtonPress;
		iniB.OnButtonPress+=OnButtonPress;
		endB.OnButtonPress+=OnButtonPress;
		endB.OnButtonUp+=OnButtonUp;
	}
	
	// Update is called once per frame
	void Update () {
		if(fireB.IsPress)
			t.forceSpeed=power;
		else
			t.forceSpeed=0;
		float[] datas=grapher.Datas.ToArray();
		gWall.SetData(datas);
	}
	
	void OnButtonPress(ButtonControl control,GameObject hit){
		if(control==iniB)
			grapher.rb=hit.rigidbody;
		if(control==fireB&&iniB.IsPress){
			grapher.StartLog();}
		if(control==endB)
			grapher.StopLog();
	}

	void OnButtonUp(ButtonControl control,GameObject hit){
		grapher.Reset();
	}
}
