using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Connector
{
	List<NetMessage> msgs=new List<NetMessage>();

	public void Send(NetMessage msg){
		msgs.Add(msg);
		Hashtable data = new Hashtable();
		data.Add("guid",msg.GetID());
		msg.OnSend(data);
	}


	public void OnReceive(SFSObject data){
		string guid=data.GetString("guid");
		for(int i =0;i<msgs.Count;i++){
			NetMessage m=msgs[i];
			if(m.GetID()==guid)
			{
				msgs.Remove(m);
				m.OnReceiveData(data);
			}
		}
	}



}


