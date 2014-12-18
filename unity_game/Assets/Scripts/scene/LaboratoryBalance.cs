using UnityEngine;
using System.Collections;

public class LaboratoryBalance : MonoBehaviour {

	[SerializeField]
	ButtonControl button1,button2;
	public bool isBalanced=false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		isBalanced=(button1.IsPress&&button2.IsPress);
	}

}
