using UnityEngine;
using System.Collections;

public class PlayerBeheaver : InputListener {
	
	GrapDetector grapDetect;
	IKController ikControl;
	GameObject _detected_object;
	bool _buttonBPress;
	float _horizontal;
	float _vertical;
	
	// Use this for initialization
	void Start () {
		grapDetect=GetComponent<GrapDetector>();
		ikControl=GetComponent<IKController>();
		grapDetect.onObjectEnter+=onObjectDetectEnter;
		grapDetect.onObjectLeave+=onObjectDetectLeave;
	}
	
	// Update is called once per frame
	void Update () {
		_horizontal=Horizontal;
		_vertical=Vertical;
		_buttonBPress=ButtonBPress;
		if(CanGrap(_detected_object)&&_buttonBPress){
			ikControl.isActive=true;
			ikControl.rightHandTarget=_detected_object.transform;
		}
		if(!_buttonBPress)
			ikControl.isActive=false;
	}
	
	bool CanGrap(GameObject grapingObj){
		if(grapingObj==null)
			return false;
		float dis=Vector3.Distance(grapingObj.transform.position,transform.position);
		//Debug.Log(dis);
		return dis<1.3f;
	}

	void onObjectDetectEnter(GameObject obj){
		_detected_object=obj;
	}

	void onObjectDetectLeave(GameObject obj){
		_detected_object=null;
	}

	public bool isSame(){
		return
			_horizontal==Horizontal&&
				_vertical==Vertical&&
				_buttonBPress==ButtonBPress;
	}
	
	public string GrapObjectName{get{
			if(_detected_object==null)
				return null;
			else
				return _detected_object.name;
		}}
}
