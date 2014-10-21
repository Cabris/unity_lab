using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientPlayerCommand : MonoBehaviour
{
	//public	bool _left,_right,_up,_down;
	public ScenePlayerCommand spc;
	PlayerBeheaver player;

	void Start ()
	{
		spc=GetComponent<ScenePlayerCommand>();
		player=GetComponent<PlayerBeheaver>();
	}
	
	void FixedUpdate ()
	{ 
		if(!player.isSame()){
			Hashtable data=new Hashtable();
			data.Add("cmd", "m");
			data.Add("horizontal", ""+player.Horizontal);
			data.Add("vertical", ""+player.Vertical);
			data.Add("buttonB", player.ButtonBPress);
			data.Add("object_name", this.name);
			data.Add("detected_object_name",player.GrapObjectName);
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
	

	
}
