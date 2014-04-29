using UnityEngine;
using System.Collections;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class NetworkController : MonoBehaviour
{
	
	protected static SmartFoxClient smartFoxClient;
	
	public static SmartFoxClient GetClient ()
	{
		return SmartFox.Connection;
	}

	void FixedUpdate ()
	{
		if (started) {
			smartFoxClient.ProcessEventQueue ();
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
		//We assume here that we have only one room - so no need to check roomId
		SendMessage ("UserEnterRoom", user);
	}
	
	// This will be invoked when a remote player lefts our room
	virtual protected  void OnUserLeaveRoom (int roomId, int userId, string userName)
	{
		SendMessage ("UserLeaveRoom", userId);
	}
	
	// Here we process incoming SFS objects
	virtual protected  void OnObjectReceived (SFSObject data, User fromUser)
	{
		
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
	
	virtual protected  void SendAnimationMessageToRemotePlayerObject (SFSObject data, User fromUser)
	{
		int userId = fromUser.GetId ();
		if (userId != smartFoxClient.myUserId) {  // If it's not myself
			//Find user object with such Id
			GameObject user = GameObject.Find ("remote_" + userId);
			//If found - send him animation message
			if (user)
				user.SendMessage ("PlayAnimation", data.GetString ("mes"));
		}
	}

	virtual protected void onExtensionResponse(object obj,string type){
		Debug.Log(type);

		Debug.Log(obj.GetType());
		Debug.Log("onExtensionResponse");
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
	
	#endregion Events

	
}
