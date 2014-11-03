using UnityEngine;
using System.Collections;


public class PlayerBeheaver : InputListener {

	public bool IsMouseSelectActive;
	IKController ikControl;
	GameObject detected_object;
	public bool ButtonBPress{get; private set;}
	float _horizontal;
	float _vertical;
	[SerializeField]
	SelectController select;
	KinectModelControllerV2 kinectModelController;
	GrapDetector grapDetector;
	[SerializeField]
	Transform grapTarget;

	void Start () {
		grapDetector = GetComponent<GrapDetector> ();
		ikControl=GetComponent<IKController>();
		grapDetector.onObjectEnter+=onObjectDetectEnter;
		grapDetector.onObjectLeave+=onObjectDetectLeave;
		select=GameObject.Find("SceneLogic").GetComponent<SelectController>();
		kinectModelController = GetComponent<KinectModelControllerV2> ();

		IsMouseSelectActive = !KinectSensor.IsInitialized;
		if (kinectModelController != null) {
			if (IsMouseSelectActive) {
				kinectModelController.enabled = false;
				grapDetector.target = grapTarget;
				grapDetector.myCamera=GameObject.Find("UI Camera").camera;
			} else {
				kinectModelController.enabled = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		_horizontal=Horizontal;
		_vertical=Vertical;
	}

	void LateUpdate ()
	{
	}
	
	bool CanGrap(GameObject grapingObj){
		if(grapingObj==null)
			return false;
		float dis=Vector3.Distance(grapingObj.transform.position,transform.position);
		return dis<1.3f;
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
				//if(CanGrap(detected_object)));
			}
			break;
		case KeyCode.B:
			ButtonBPress=true;
			if(detected_object!=null)
				select.onSelect(detected_object);
			else
				select.onUnselectAll();
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
	
	public bool isSame(){
		return
			_horizontal==Horizontal&&
				_vertical==Vertical;
//				_buttonBPress==ButtonBPress;
	}
	
	public string GrapObjectName{get{
			if(detected_object==null)
				return null;
			else
				return detected_object.name;
		}}
}
