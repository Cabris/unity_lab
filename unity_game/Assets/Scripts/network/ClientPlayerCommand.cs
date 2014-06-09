using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientPlayerCommand : InputListener
{
	//public	bool _left,_right,_up,_down;
	public bool _buttonBPress;
	public float _horizontal;
	public float _vertical;
	public ScenePlayerCommand spc;
	GrapDetector grapDetect;
	GameObject _detected_object;
	// Use this for initialization
	void Start ()
	{
		spc=GetComponent<ScenePlayerCommand>();
		grapDetect=GetComponent<GrapDetector>();
	}
	
	void FixedUpdate ()
	{ 
		if(!isSame()){
			_horizontal=Horizontal;
			_vertical=Vertical;
			_buttonBPress=ButtonBPress;
			_detected_object=grapDetect.detectedObj;
			Hashtable data=new Hashtable();
			data.Add("cmd", "m");
			data.Add("horizontal", ""+_horizontal);
			data.Add("vertical", ""+_vertical);
			data.Add("buttonB", _buttonBPress);
			data.Add("object_name", this.name);
			if(_detected_object!=null)
				data.Add ("detected_object_name",grapDetect.detectedObj.name);
			else
				data.Add ("detected_object_name",null);
			if(NetworkController.GetClient()!=null){
				ClientNetworkController.SendExMsg("test","s",data);
				GetComponent<Animator>().applyRootMotion=false;
			}
			else if(spc!=null){
				spc.ReceiveCommand(data.ToSFSObject());
				GetComponent<Animator>().applyRootMotion=true;
			}
		}
	}
	
	bool isSame(){
		bool gd=_detected_object==grapDetect.detectedObj;

		return
			_horizontal==Horizontal&&
				_vertical==Vertical&&
				_buttonBPress==ButtonBPress&&
				gd;
	}
	
}
