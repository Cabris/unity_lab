using UnityEngine;
using System.Collections;

public class AudienceCamera : MonoBehaviour {
	[SerializeField]
	Camera leftCam,rightCam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform .position=.5f*(leftCam.transform.position+rightCam.transform.position);
	}
}
