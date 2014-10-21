using UnityEngine;
using System.Collections;

public class MouseSelectController : MonoBehaviour
{
	public bool IsActive;
	KinectModelControllerV2 kinectModelController;
	GrapDetector grapDetector;
	Vector3 pos = new Vector3 ();
	// Use this for initialization
	void Start ()
	{
		kinectModelController = GetComponent<KinectModelControllerV2> ();
		grapDetector = GetComponent<GrapDetector> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		IsActive = !KinectSensor.IsInitialized;
		if (kinectModelController != null) {
			if (IsActive) {
				kinectModelController.enabled = false;
			} else {
				kinectModelController.enabled = true;
			}
		}
		int layerMask = 1 << 15;//plane
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100, Color.blue);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100, layerMask)) {
			pos = hit.point;
		}
		
	}
	
	void LateUpdate ()
	{
		if (IsActive) {
			grapDetector.target.position = pos;
		}
	}
}
