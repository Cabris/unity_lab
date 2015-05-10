using UnityEngine;
using System.Collections;
using System;

public class PlayerBeheaver : InputListener {
	
	public enum ControlType{mono,stereo };
	public ControlType controlType;
	[SerializeField]
	Transform corsor,hand, crosshair, qr;
	SelectController select;
	IKController ikControl;
	GameObject detected_object;
	KinectModelControllerV2 kinectModelController;
	GrapDetector grapDetector;
	PlayerAnimation playerAni;
	bool isT=false;
	Vector3 initialPos;
	[SerializeField]
	Camera main,left,right,mainSpace,leftSpace,rightSpace,qrC;
	MonoController monoInput;
	WiiController wiiInput;

	public string playerId;

	void Start () {
		DateTime now=DateTime.UtcNow;

		playerId="subject-"+now.ToString("yyMMddHHmm");
		initialPos=transform.position;
		grapDetector = GetComponent<GrapDetector> ();
		ikControl=GetComponent<IKController>();
		grapDetector.onObjectEnter+=onObjectDetectEnter;
		grapDetector.onObjectLeave+=onObjectDetectLeave;
		select=GameObject.Find("SceneLogic").GetComponent<SelectController>();
		kinectModelController = GetComponent<KinectModelControllerV2> ();
		playerAni=GetComponent<PlayerAnimation>();
		bool isKinectInited= KinectSensor.IsInitialized;
		
		monoInput=GetComponent<MonoController>();
		wiiInput=GetComponent<WiiController>();
		
		if(controlType==ControlType.mono){
			monoInput.enabled=true;
			wiiInput.enabled=false;
			grapDetector.target = corsor;

			left.depth=-50;
			leftSpace.depth=-50;
			right.depth=-50;
			rightSpace.depth=-50;
			main.depth=10;
			mainSpace.depth=0;
			qrC.depth=-50;

		}else{
			monoInput.enabled=false;
			wiiInput.enabled=true;
			grapDetector.target = hand;

			mainSpace.depth=-50;
			main.depth=-50;

			leftSpace.depth=0;
			left.depth=10;
			rightSpace.depth=20;
			right.depth=30;
			qrC.depth=40;
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
		if (controlType == ControlType.stereo) {
			crosshair.position = grapDetector.end.position;	
			crosshair.rotation=grapDetector.end.rotation;
		} else {
			Camera uicamera=Camera.main;
			Vector3 p= new Vector3(Screen.width/2,
			                       Screen.height/2, uicamera.nearClipPlane+.1f);
			crosshair.position = uicamera.ScreenToWorldPoint(p);
			qr.localScale=Vector3.zero;
			crosshair.localScale=new Vector3(.13f,.13f,1);
		}
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

	public void onConnected(){
		qrC.depth=-50;
	}

	public void DoDamage(float d){
		if (d > 0) {
			isT = true;
		}
	}
		
}
