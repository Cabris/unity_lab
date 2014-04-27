using UnityEngine;
using System.Collections;

using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ServerNetworkController : MonoBehaviour {

	private static SmartFoxClient smartFoxClient;
	
	public static SmartFoxClient GetClient() {
		return SmartFox.Connection;
	}

	public NetworkTransformSender[] propsSender; 


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
			Application.LoadLevel("login");
			return;
		}	
		SubscribeEvents();
		started = true;
		smartFoxClient.JoinRoom("Central Square");
		foreach(NetworkTransformSender r in propsSender){
			r.StartSending();
			r.ForceSendTransform();
		}
	}
	
	// We should unsubscribe all delegates before quitting the application to avoid probleems.
	// Also we should Disconnect from server
	void OnApplicationQuit() {
		UnsubscribeEvents();
		smartFoxClient.Disconnect();
	}
	
	
	private void OnJoinRoom(Room room) {
		//SendMessage("SpawnPlayers");
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
		Debug.Log("OnObjectReceived: "+_cmd);
		switch (_cmd) {
		case "t":  // "t" - means transform sync data
			SendTransformToRemoteObject(data, fromUser);
			break;
		case "a": // "a" - for animation message received
			SendAnimationMessageToRemotePlayerObject(data, fromUser);
			break;
		case "m": // "a" - for animation message received
			SendCommandToServerPlayerObject(data, fromUser);
			break;
		case "f": // "a" - for animation message received
			ForceSendTransform(data);
//			Debug.Log("ForceSendTransform");
			break;
		}

	}
	
	private void SendTransformToRemoteObject(SFSObject data, User fromUser) {
		int userId = fromUser.GetId();
		string userName= data.GetString("object_name");
		if (userId!=smartFoxClient.myUserId) {  // If it's not myself
			//Find user object with such Id
			GameObject user = GameObject.Find(userName);
			//If found - send him message with transform data
			if (user!=null&&user.GetComponent<serverPlayerCommand>()!=null){
				user.SendMessage("ReceiveTransform", data);
			}
		}
	}

	private void SendCommandToServerPlayerObject(SFSObject data, User fromUser) {
		int userId = fromUser.GetId();
		string objName= data.GetString("object_name");
		if (userId!=smartFoxClient.myUserId) {  // If it's not myself
			//Find user object with such Id
			GameObject user = GameObject.Find(objName);
//			Debug.Log("SendCommandToServerPlayerObject: "+objName);
			//If found - send him message with transform data
			if (user!=null&&user.GetComponent<serverPlayerCommand>()!=null){
				user.SendMessage("ReceiveCommand", data);
				Debug.Log("SendCommandToServerPlayerObject: "+objName);
			}
		}
	}

	private void ForceSendTransform(SFSObject data) {
		//if this message is addressed to this user
		string objName= data.GetString("object_name");
		if (data.GetString("to_user") == "scene") {
			// Find local player object
			GameObject user = GameObject.Find(objName);
			// Send him message
			if (user&&user.GetComponent<NetworkTransformSender>()!=null) {
				user.SendMessage("ForceSendTransform");
				Debug.Log("ForceSendTransform: "+ objName);
			}
		}
	}

//	private void ForceSendTransform(SFSObject data) {
//		//if this message is addressed to this user
//		if ((int)data.GetNumber("to_uid") == smartFoxClient.myUserId) {
//			// Find local player object
//			GameObject user = GameObject.Find("localPlayer");
//			// Send him message
//			if (user) user.SendMessage("ForceSendTransform");
//		}
//	}
	
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
