using UnityEngine;
using System.Collections;

public class StirlingEngineController : MonoBehaviour {
	MechinePart[] ms;
	public ConstantForce wheelForse;
	// Use this for initialization
	void Start () {
		ms=GetComponentsInChildren<MechinePart>();
		wheelForse=gameObject.GetComponentInChildren<ConstantForce>();
	}
	
	// Update is called once per frame
	void Update () {
		bool isComplete=true;
		foreach(MechinePart m in ms){
			isComplete=(isComplete&&m.isPlaced);
		}
		if(isComplete)
			wheelForse.torque=new Vector3(0,0,20);
		else
			wheelForse.torque=new Vector3(0,0,0);
	}
}
