using System;
using System.Collections;
using System.Collections.Generic;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;
public class NetMessage
{
	Guid g;
	public NetMessage(){
		g=Guid.NewGuid();
	}
	public delegate void ReceiveDataEvent(SFSObject data);
	public ReceiveDataEvent OnReceiveData;

	public delegate void SendEvent(Hashtable data);
	public SendEvent OnSend;
	public string GetID(){
		return g.ToString();
	}


}


