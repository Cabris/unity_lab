using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public SmoothFollow follow; 
	public float dragSpeed = 2;
	private Vector3 dragOrigin;
	float iniCameraHeight;
	Quaternion iniRotation;
	float iniDistance;
	[SerializeField]
	float maxH,minH,maxD,minD;
	// Use this for initialization
	void Start () {
		iniCameraHeight=follow.height;
		iniRotation=transform.localRotation;
		iniDistance=follow.distance;
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

		follow.distance-=y;
		if(follow.distance>maxD)
			follow.distance=maxD;
		if(follow.distance<minD)
			follow.distance=minD;

		follow.height-=y;
		if(follow.height>maxH)
			follow.height=maxH;
		if(follow.height<minH)
			follow.height=minH;

	}

	public void Reset(){
		transform.localRotation=iniRotation;
		follow.height=iniCameraHeight;
		follow.distance=iniDistance;
	}

}














