using UnityEngine;
using System.Collections;

public class GrapDetector : MonoBehaviour {
	public  Transform target;
	public float distance;
	//public Transform temp;
	public GameObject detectedObj;
	Vector3 posHand;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 handPosInScreen=Camera.main.WorldToScreenPoint(posHand);
		Vector3 posC=Camera.main.transform.position;
		Vector3 direction=(posHand-posC).normalized;
		//direction=Camera.main.transform.rotation.eulerAngles;
		direction.Normalize();
		int layerMask=1<<10;
		Ray ray=Camera.main.ScreenPointToRay(handPosInScreen);
		ray=new Ray(posHand,direction);
		Debug.DrawRay(ray.origin, direction, Color.red);

		RaycastHit hit;
		//temp.position=posHand+direction*distance;
		if (Physics.Raycast(ray, out hit,distance,layerMask)){
			detectedObj=hit.collider.gameObject;
		}else
			detectedObj=null;
	}
	
	void LateUpdate () {
		 posHand=target.position;
	}
}
