using UnityEngine;
using System.Collections;

public class PlayerControllerKinect : MonoBehaviour {
	KinectModelControllerV2 kmc;
	// Use this for initialization
	void Start () {
		kmc=GetComponent<KinectModelControllerV2>();
		KinectModelControllerV2.BoneMask mask=
			KinectModelControllerV2.BoneMask.Upper_Body;
		kmc.Mask=mask;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
