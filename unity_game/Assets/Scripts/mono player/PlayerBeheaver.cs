using UnityEngine;
using System.Collections;


public class PlayerBeheaver : InputListener {
	public enum ControlType{mono,stereo };
	
	public ControlType controlType;
	[SerializeField]
	Transform grapTarget;
	SelectController select;
	IKController ikControl;
	GameObject detected_object;
	KinectModelControllerV2 kinectModelController;
	GrapDetector grapDetector;
	PlayerAnimation playerAni;
	bool isT=false;
	Vector3 initialPos;

	MonoController monoInput;
	WiiController wiiInput;
	
	void Start () {
		initialPos=transform.position;
		grapDetector = GetComponent<GrapDetector> ();
		ikControl=GetComponent<IKController>();
		grapDetector.onObjectEnter+=onObjectDetectEnter;
		grapDetector.onObjectLeave+=onObjectDetectLeave;
		select=GameObject.Find("SceneLogic").GetComponent<SelectController>();
		kinectModelController = GetComponent<KinectModelControllerV2> ();
		playerAni=GetComponent<PlayerAnimation>();
		bool isKinectInited= KinectSensor.IsInitialized;
//		IsMouseSelectActive = !KinectSensor.IsInitialized;
//		if (kinectModelController != null) {
//			if (IsMouseSelectActive) {
//				kinectModelController.enabled = false;
//				grapDetector.target = grapTarget;
//			} else {
//				kinectModelController.enabled = true;
//			}
//		}
		grapDetector.target = grapTarget;
		monoInput=GetComponent<MonoController>();
		wiiInput=GetComponent<WiiController>();

		if(controlType==ControlType.mono){
			monoInput.enabled=true;
			wiiInput.enabled=false;
		}else{
			monoInput.enabled=false;
			wiiInput.enabled=true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		//base.Update();
		playerAni.direction=Horizontal;
		playerAni.speed=Vertical;
		
		if (isT) {
			playerAni.Disable();
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;

			StartCoroutine(reset(5));
			isT=false;		
		}

		if(detected_object!=null&&GetButtonDown("Fire3"))
			select.onSelect(detected_object);

	}

	IEnumerator  reset(float s){
		yield return new WaitForSeconds(s);
		rigidbody.isKinematic = false;
		rigidbody.detectCollisions = true;
		transform.position=initialPos;
		playerAni.Enable();
	}

	void onObjectDetectEnter(GameObject obj){
		detected_object=obj;
	}
	
	void onObjectDetectLeave(GameObject obj){
		detected_object=null;
	}

	public string GrapObjectName{
		get{
			if(detected_object==null)
				return null;
			else
				return detected_object.name;
		}
	}
	
	public void DoDamage(float d){
		if (d > 0) {
			isT = true;
		}
	}
	

}
