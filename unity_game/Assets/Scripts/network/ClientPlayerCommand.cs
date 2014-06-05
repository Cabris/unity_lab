using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientPlayerCommand : InputListener
{
	public	bool _left,_right,_up,_down;
	public float h;
	public float v;
	public ScenePlayerCommand spc;
	// Use this for initialization
	void Start ()
	{
		spc=GetComponent<ScenePlayerCommand>();
	}
	
	void FixedUpdate ()
	{ 
		h=Horizontal;
		v=Vertical;

		if(!isSame()){

			_left=Horizontal<0;
			_right=Horizontal>0;
			_up=Vertical>0;
			_down=Vertical<0;
			
			Hashtable data=new Hashtable();
			data.Add("cmd", "m");
			data.Add ("left", _left);
			data.Add ("right", _right);
			data.Add ("up", _up);
			data.Add ("down", _down);
			data.Add("object_name", this.name);

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
		return
			_left==Horizontal<0&&
			_right==Horizontal>0&&
			_up==Vertical>0&&
			_down==Vertical<0;
	}
	
}
