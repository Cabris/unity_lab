using UnityEngine;
using System.Collections;

public class MouseBoneController : MonoBehaviour {
	public bool IsActive;
	public KinectModelControllerV2 kinectModelController;
	public GrapDetector grapDetector;
	Ray ray;
	public float tempD;
	// Use this for initialization
	void Start () {
		kinectModelController=GetComponent<KinectModelControllerV2>();
		grapDetector=GetComponent<GrapDetector>();
		ray=Camera.main.ScreenPointToRay(Input.mousePosition);

	}
	
	// Update is called once per frame
	void Update () {
		IsActive=!KinectSensor.IsInitialized;
		if(kinectModelController!=null){
			if(IsActive){
				kinectModelController.enabled=false;
			}else{
				kinectModelController.enabled=true;
			}
		}

		ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction*100, Color.blue);
	}

	void LateUpdate() {
		if(IsActive){
			grapDetector.target.position=ray.origin+ray.direction*tempD;
		}
	}
}
