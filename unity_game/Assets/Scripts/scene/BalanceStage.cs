using UnityEngine;
using System.Collections;

public class BalanceStage : MonoBehaviour {
	public LaboratoryBalance balance;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(balance.Balance-270f)<0.01f){
			this.rigidbody.isKinematic=false;
			this.rigidbody.WakeUp();
		}
	}
}
