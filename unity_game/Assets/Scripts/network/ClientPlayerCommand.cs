using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientPlayerCommand : MonoBehaviour
{
	WiiController wiiController;
	bool _left,_right,_up,_down;
	
	// Use this for initialization
	void Start ()
	{
		wiiController=GetComponent<WiiController>();
//		axis=new Vector4();
	}
	
	void FixedUpdate ()
	{ 
		if(wiiController!=null&&!isSame()){
			

			_left=wiiController.left;
			_right=wiiController.right;
			_up=wiiController.up;
			_down=wiiController.down;
			
			Hashtable data=new Hashtable();
			data.Add("cmd", "m");
			data.Add ("left", _left);
			data.Add ("right", _right);
			data.Add ("up", _up);
			data.Add ("down", _down);
			data.Add("object_name", this.name);
			ClientNetworkController.SendExMsg("test","s",data);
		}
	}
	
	bool isSame(){
		return
			_left==wiiController.left&&
				_right==wiiController.right&&
				_up==wiiController.up&&
				_down==wiiController.down;
		
	}
	
	
}
