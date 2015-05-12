using UnityEngine;
using System.Collections;

public class BalanceStage : MonoBehaviour {
	[SerializeField]
	LaboratoryBalance[] balances;
	[SerializeField]
	SwitchWall wall;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool isB=true;
		foreach(LaboratoryBalance b in balances)
			isB&=b.isBalanced;
		wall.Open=isB;
	}
}
