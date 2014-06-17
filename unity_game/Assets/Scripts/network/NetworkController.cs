using UnityEngine;
using System.Collections;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class NetworkController : MonoBehaviour
{
	protected static MyUserType userType=MyUserType.Undefine;
	public static MyUserType UserType{get{return userType;}}
	protected static SmartFoxClient smartFoxClient;
	public static SmartFoxClient GetClient ()
	{
		return SmartFox.Connection;
	}
	
	void FixedUpdate ()
	{
		if (started) {
			smartFoxClient.ProcessEventQueue ();
			if(Time.time%10==0)
				SceneMenu.RequestSceneInfo();
		}
	}
	
	// We should unsubscribe all delegates before quitting the application to avoid probleems.
	// Also we should Disconnect from server
	void OnApplicationQuit ()
	{
		UnsubscribeEvents ();
		smartFoxClient.Disconnect ();
	}
	
	virtual protected void OnJoinRoom (Room room)
	{
		string extensionName="test";
		Hashtable msg = new Hashtable ();
		msg.Add ("#",GetClient().myUserName+ ": OnJoinRoom("+room.GetName()+")");
		GetClient ().SendXtMessage (extensionName, "test", msg);
	}
	
	// This will be invoked when remote player enters our room
	virtual protected void OnUserEnterRoom (int roomId, User user)
	{

	}
	
	// This will be invoked when a remote player lefts our room
	virtual protected  void OnUserLeaveRoom (int roomId, int userId, string userName)
	{

	}
	
	// Here we process incoming SFS objects
	virtual protected  void OnObjectReceived (SFSObject data, User fromUser)
	{
		HandleReceiveData(data);
	}

	public void Send(string to,SFSObject data){
	//	SmartFoxClient client = GetClient ();
	//	client.SendObject(data);
	}
	
	virtual public  void OnPublicMessage (string message, User fromUser, int roomId)
	{
		int userId = fromUser.GetId ();
		if (userId != smartFoxClient.myUserId) {  // If it's not myself
			string mes = fromUser.GetName () + ": " + message;
			// Send chat message to the Chat Controller			
			SendMessage ("AddChatMessage", mes);
			//Find user object with such Id
			GameObject user = GameObject.Find ("remote_" + userId);
			//If found - send him bubble message
			if (user) {
				user.SendMessage ("ShowBubble", mes);
			}
		}
	}

	virtual protected void onExtensionResponse(object obj,string type){
		SFSObject data=obj as SFSObject;
		HandleReceiveData(data);
	}

	public static void SendExMsg(string exName,string cmd,Hashtable data){
		SmartFoxClient client = GetClient();
		if(client!=null)
			client.SendXtMessage(exName,cmd,data);
	}

	#region Events
	
	protected bool started = false;
	
	protected void SubscribeEvents ()
	{
		SFSEvent.onJoinRoom += OnJoinRoom;
		SFSEvent.onUserEnterRoom += OnUserEnterRoom;
		SFSEvent.onUserLeaveRoom += OnUserLeaveRoom;
		SFSEvent.onObjectReceived += OnObjectReceived;
		SFSEvent.onPublicMessage += OnPublicMessage;
		SFSEvent.onExtensionResponse+=onExtensionResponse;
	}
	
	protected void UnsubscribeEvents ()
	{
		SFSEvent.onJoinRoom -= OnJoinRoom;
		SFSEvent.onUserEnterRoom -= OnUserEnterRoom;
		SFSEvent.onUserLeaveRoom -= OnUserLeaveRoom;
		SFSEvent.onObjectReceived -= OnObjectReceived;
		SFSEvent.onPublicMessage -= OnPublicMessage;
		SFSEvent.onExtensionResponse-=onExtensionResponse;
	}

	virtual protected void HandleReceiveData(SFSObject data){}
	
	#endregion Events

}

public enum MyUserType{
	Undefine,
	Scene,
	Client
}
