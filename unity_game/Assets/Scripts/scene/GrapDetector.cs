using UnityEngine;
using System.Collections;

public class GrapDetector : MonoBehaviour {
	public Transform target{ get; set;}
	float distance;
	public Transform start;
	public Transform end;
	GameObject pre_obj;
	GameObject detectedObj;
	Vector3 handPos;
	public Camera myCamera{ get; set;} 
	public delegate void OnObjectDetectEvent(GameObject obj);
	public OnObjectDetectEvent onObjectEnter,onObjectLeave;
	
	// Use this for initialization
	void Start () {
		distance=999;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 handPosInScreen=myCamera.WorldToScreenPoint(handPos);
		int layerMask=1<<10;
		Ray ray=Camera.main.ScreenPointToRay(handPosInScreen);
		//ray=new Ray(handPos,direction);
		Debug.DrawRay(ray.origin, ray.direction*distance, Color.red);
		
		RaycastHit hit;
		start.position=handPos;
		end.position=handPos+ray.direction*distance;
		
		if (Physics.Raycast(ray, out hit,distance,layerMask)){
			detectedObj=hit.collider.gameObject;
			if(pre_obj!=detectedObj){//state change
				if(pre_obj!=null&&onObjectLeave!=null)//from exist obj to another
					onObjectLeave(pre_obj);//leave old one
				if(onObjectEnter!=null)
					onObjectEnter(detectedObj);
			}
			pre_obj=detectedObj;
			
		}else{//go to empty
			if(onObjectLeave!=null&&detectedObj!=null)
				onObjectLeave(detectedObj);
			detectedObj=null;
			pre_obj=null;
		}
	}
	
	void LateUpdate () {
		handPos=target.position;
	}
}
