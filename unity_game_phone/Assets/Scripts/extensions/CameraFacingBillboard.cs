//cameraFacingBillboard.cs v02
//by Neil Carter (NCarter)
//modified by Juan Castaneda (juanelo)
//
//added in-between GRP object to perform rotations on
//added auto-find main camera
//added un-initialized state, where script will do nothing
using UnityEngine;
using System.Collections;


public class CameraFacingBillboard : MonoBehaviour
{
	
	public Camera m_Camera;
	public Transform cameraTF;
	public bool amActive =false;
	public bool autoInit =false;
	GameObject myContainer;	
	bool inited=false;
	void Awake(){
		if (autoInit == true&&!inited)
			Init();
	}

	public Transform Init(){
		inited=true;
		m_Camera = Camera.main;
		cameraTF=m_Camera.transform;
		amActive = true;
		myContainer = new GameObject();
		myContainer.transform.parent=transform.parent;
		myContainer.name = "GRP_"+transform.gameObject.name;
		myContainer.transform.position = transform.position;
		transform.parent = myContainer.transform;
		return myContainer.transform;
	}
	
	void Update(){
		if(amActive==true){
			myContainer.transform.LookAt(myContainer.transform.position + cameraTF.rotation * Vector3.forward, cameraTF.rotation * Vector3.up);
		}
	}
}