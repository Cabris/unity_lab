using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public SmoothFollow follow; 
	public float dragSpeed = 2;
	private Vector3 dragOrigin;
	float iniCameraHeight;
	Quaternion iniRotation;
	// Use this for initialization
	void Start () {
		iniCameraHeight=follow.height;
		iniRotation=transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.R))
		{
			Reset();
			return;
		}

		if (Input.GetMouseButtonDown(1))
		{
			dragOrigin = Input.mousePosition;
			return;
		}
		
		if (!Input.GetMouseButton(1)) return;
		
		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 r=new Vector3(0,pos.x * dragSpeed,0);
		transform.Rotate(r,Space.Self);
		float y=pos.y*dragSpeed*0.05f;
		follow.height-=y;
		if(follow.height>3)
			follow.height=3;
		if(follow.height<-1)
			follow.height=-1;

	}

	public void Reset(){
		transform.rotation=iniRotation;
		follow.height=iniCameraHeight;
	}

}














