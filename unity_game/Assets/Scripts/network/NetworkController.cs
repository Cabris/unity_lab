using UnityEngine;
using System.Collections;

using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class NetworkController : MonoBehaviour {
	
	private static SmartFoxClient smartFoxClient;
	
	public static SmartFoxClient GetClient() {
		return SmartFox.Connection;
	}
	
	#region Events
	
	private bool started = false;
	
	private void SubscribeEvents() {
		SFSEvent.onJoinRoom += OnJoinRoom;
		SFSEvent.onUserEnterRoom += OnUserEnterRoom;
		SFSEvent.onUserLeaveRoom += OnUserLeaveRoom;
		SFSEvent.onObjectReceived += OnObjectReceived;
		SFSEvent.onPublicMessage += OnPublicMessage;
    }
	
	private void UnsubscribeEvents() {
		SFSEvent.onJoinRoom -= OnJoinRoom;
		SFSEvent.onUserEnterRoom -= OnUserEnterRoom;
		SFSEvent.onUserLeaveRoom -= OnUserLeaveRoom;
		SFSEvent.onObjectReceived -= OnObjectReceived;
		SFSEvent.onPublicMessage -= OnPublicMessage;
	}
	
	void FixedUpdate() {
		if (started) {
			smartFoxClient.ProcessEventQueue();
		}
	}
	
	#endregion Events
	
	// We start working from here
	void Start() {
		Application.runInBackground = true; // Let the application be running whyle the window is not active.
		smartFoxClient = GetClient();
		if (smartFoxClient==null) {
			Application.LoadLevel("sc_Login");
			return;
		}	
		SubscribeEvents();
		started = true;
		smartFoxClient.JoinRoom("Central Square");
	}
	
	// We should unsubscribe all delegates before quitting the application to avoid probleems.
	// Also we should Disconnect from server
	void OnApplicationQuit() {
		UnsubscribeEvents();
		smartFoxClient.Disconnect();
	}
	
	
	private void OnJoinRoom(Room room) {
		SendMessage("SpawnPlayers");
		Debug.Log("Connected !");
	}
	
	// This will be invoked when remote player enters our room
	private void OnUserEnterRoom(int roomId, User user)
	{
		//We assume here that we have only one room - so no need to check roomId
		SendMessage("UserEnterRoom", user);
	}
	
	// This will be invoked when a remote player lefts our room
	private void OnUserLeaveRoom(int roomId, int userId, string userName)
	{
		SendMessage("UserLeaveRoom", userId);
	}
	
	// Here we process incoming SFS objects
	private void OnObjectReceived(SFSObject data, User fromUser) {
		//First determine the type of this object - what it contains ?
		String _cmd = data.GetString("_cmd");
		switch (_cmd) {
			case "t":  // "t" - means transform sync data
				SendTransformToRemotePlayerObject(data, fromUser);
				break;
			case "f":  // "f" - means force our local player to send his transform
				ForceSendTransform(data);
				break;
			case "a": // "a" - for animation message received
				SendAnimationMessageToRemotePlayerObject(data, fromUser);
				break;
		}
	}
	
	private void SendTransformToRemotePlayerObject(SFSObject data, User fromUser) {
		int userId = fromUser.GetId();
		if (userId!=smartFoxClient.myUserId) {  // If it's not myself
			//Find user object with such Id
			GameObject user = GameObject.Find("remote_"+userId);
			
			//If found - send him message with transform data
			if (user&&data.GetString("object_name")==("client_"+user.name)) 
				user.SendMessage("ReceiveTransform", data);
		}
	}
	
	private void ForceSendTransform(SFSObject data) {
		//if this message is addressed to this user
		if ((int)data.GetNumber("to_uid") == smartFoxClient.myUserId) {
			// Find local player object
			GameObject user = GameObject.Find("localPlayer");
			// Send him message
			if (user) user.SendMessage("ForceSendTransform");
		}
	}
	
	private void SendAnimationMessageToRemotePlayerObject(SFSObject data, User fromUser) {
		int userId = fromUser.GetId();
		if (userId!=smartFoxClient.myUserId) {  // If it's not myself
			//Find user object with such Id
			GameObject user = GameObject.Find("remote_"+userId);
			//If found - send him animation message
			if (user) user.SendMessage("PlayAnimation", data.GetString("mes"));
		}
	}
	
	public void OnPublicMessage(string message, User fromUser, int roomId)
	{
		int userId = fromUser.GetId();
    	if (userId!=smartFoxClient.myUserId) {  // If it's not myself
    		string mes = fromUser.GetName()+": "+message;
    	
			// Send chat message to the Chat Controller			
			SendMessage("AddChatMessage", mes);
			
			//Find user object with such Id
			GameObject user = GameObject.Find("remote_"+userId);
			//If found - send him bubble message
			if (user) {
				user.SendMessage("ShowBubble", mes);
			}
		}
	}
		
}
