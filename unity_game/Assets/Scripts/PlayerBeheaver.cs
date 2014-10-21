using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBeheaver : InputListener {

	[SerializeField]
	GameObject panel;
	GrapDetector grapDetect;
	IKController ikControl;
	GameObject _detected_object;
	public bool ButtonBPress{get; private set;}
	float _horizontal;
	float _vertical;
	SelectController select;
	
	// Use this for initialization
	void Start () {
		grapDetect=GetComponent<GrapDetector>();
		ikControl=GetComponent<IKController>();
		grapDetect.onObjectEnter+=onObjectDetectEnter;
		grapDetect.onObjectLeave+=onObjectDetectLeave;
		select=new SelectController();
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
		//Debug.Log(dis);
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
				selectObj(_detected_object);
			else
				unselectAll();
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


	Dictionary<GameObject,GameObject> selections=new Dictionary<GameObject, GameObject>();
	void selectObj(GameObject obj){
//		ikControl.isActive=true;
//		ikControl.rightHandTarget=obj.transform;
		if(!selections.ContainsKey(obj)){
			GameObject _p=GameObject.Instantiate(panel) as GameObject;
			_p.transform.position=obj.transform.position;
			_p.GetComponent<CameraFacingBillboard>().Init();
			selections.Add(obj,_p);
		}
	}

	void unselectAll(){
		foreach(GameObject selectObj in selections.Keys){
			GameObject p=selections[selectObj];
			GameObject.Destroy(p);
		}
		selections.Clear();
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
