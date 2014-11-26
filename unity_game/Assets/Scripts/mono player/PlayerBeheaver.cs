using UnityEngine;
using System.Collections;


public class PlayerBeheaver : InputListener {
	
	public bool ButtonBPress{get; private set;}
	[SerializeField]
	SelectController select;
	[SerializeField]
	Transform grapTarget;
	[SerializeField]
	bool IsMouseSelectActive;
	IKController ikControl;
	GameObject detected_object;
	KinectModelControllerV2 kinectModelController;
	GrapDetector grapDetector;
	PlayerAnimation playerAni;
	
	[SerializeField]
	bool isT=false;
	
	Vector3 initialPos;
	
	void Start () {
		initialPos=transform.position;
		grapDetector = GetComponent<GrapDetector> ();
		ikControl=GetComponent<IKController>();
		grapDetector.onObjectEnter+=onObjectDetectEnter;
		grapDetector.onObjectLeave+=onObjectDetectLeave;
		select=GameObject.Find("SceneLogic").GetComponent<SelectController>();
		kinectModelController = GetComponent<KinectModelControllerV2> ();
		playerAni=GetComponent<PlayerAnimation>();
		
		IsMouseSelectActive = !KinectSensor.IsInitialized;
		if (kinectModelController != null) {
			if (IsMouseSelectActive) {
				kinectModelController.enabled = false;
				grapDetector.target = grapTarget;
				//grapDetector.myCamera=GameObject.Find("UI Camera").camera;
			} else {
				kinectModelController.enabled = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		playerAni.direction=Horizontal;
		playerAni.speed=Vertical;
		
		if (isT) {
			playerAni.Disable();
			KeyboardController kc=GetComponent<KeyboardController>();
			kc.enabled=false;
			//CameraControllercs c=Camera.main.GetComponent<CameraControllercs>();
			//c.target=transform.Find ("Root/Hips").gameObject;
			StartCoroutine(reset(5));
			isT=false;		
		}
	}

	IEnumerator  reset(float s){
		yield return new WaitForSeconds(s);
		playerAni.Enable();
		KeyboardController kc=GetComponent<KeyboardController>();
		kc.enabled=true;
		transform.position=initialPos;
		//Application.LoadLevel (Application.loadedLevelName);
	}
	
	void LateUpdate ()
	{
	}

	void onObjectDetectEnter(GameObject obj){
		detected_object=obj;
	}
	
	void onObjectDetectLeave(GameObject obj){
		detected_object=null;
	}
	
	public override void OnKeyPress (KeyCode k)
	{
		base.OnKeyPress (k);
		switch(k) {
		case KeyCode.A:
			if(detected_object!=null){
			}
			break;
		case KeyCode.B:
			ButtonBPress=true;
			if(detected_object!=null)
				select.onSelect(detected_object);
			break;
		}
	}
	
	public override void OnKeyUp (KeyCode k)
	{
		base.OnKeyUp (k);
		switch(k) {
		case KeyCode.A:
			break;
		case KeyCode.B:
			ButtonBPress=false;
			break;
		}
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
