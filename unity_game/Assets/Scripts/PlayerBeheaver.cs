using UnityEngine;
using System.Collections;


public class PlayerBeheaver : InputListener {
	
	GrapDetector grapDetect;
	IKController ikControl;
	GameObject _detected_object;
	public bool ButtonBPress{get; private set;}
	float _horizontal;
	float _vertical;
	[SerializeField]
	SelectController select;


	void Start () {
		grapDetect=GetComponent<GrapDetector>();
		ikControl=GetComponent<IKController>();
		grapDetect.onObjectEnter+=onObjectDetectEnter;
		grapDetect.onObjectLeave+=onObjectDetectLeave;
		select=GameObject.Find("SceneLogic").GetComponent<SelectController>();
	}
	
	// Update is called once per frame
	void Update () {
		_horizontal=Horizontal;
		_vertical=Vertical;
//		if(CanGrap(_detected_object)&&ButtonBPress){
//			ikControl.isActive=true;
//			ikControl.rightHandTarget=_detected_object.transform;
//		}
//		if(!ButtonBPress)
//			ikControl.isActive=false;
	}
	
	bool CanGrap(GameObject grapingObj){
		if(grapingObj==null)
			return false;
		float dis=Vector3.Distance(grapingObj.transform.position,transform.position);
		return dis<1.3f;
	}

	void onObjectDetectEnter(GameObject obj){
		_detected_object=obj;
	}

	void onObjectDetectLeave(GameObject obj){
		_detected_object=null;
	}

	public override void OnKeyPress (KeyCode k)
	{
		base.OnKeyPress (k);
		switch(k) {
		case KeyCode.A:
			break;
		case KeyCode.B:
			ButtonBPress=true;
			if(_detected_object!=null)
				select.onSelect(_detected_object);
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
			if(_detected_object==null)
				return null;
			else
				return _detected_object.name;
		}}
}
