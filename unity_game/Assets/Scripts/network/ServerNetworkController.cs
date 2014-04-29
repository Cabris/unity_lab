using UnityEngine;
using System.Collections;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ServerNetworkController : NetworkController {

	public NetworkTransformSender[] propsSender;

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

	protected override void OnJoinRoom(Room room) {
		//SendMessage("SpawnPlayers");
		Debug.Log("Scene Connected !");
	}

	// Here we process incoming SFS objects
	protected  override void OnObjectReceived(SFSObject data, User fromUser) {
		//First determine the type of this object - what it contains ?
		String _cmd = data.GetString("_cmd");
		Debug.Log("OnObjectReceived: "+_cmd);
		switch (_cmd) {
		case "t":  // "t" - means transform sync data
			SendTransformToRemoteObject(data, fromUser);
			break;
//		case "a": // "a" - for animation message received
//			SendAnimationMessageToRemotePlayerObject(data, fromUser);
//			break;
		case "m": // client command
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
		if (userId!=smartFoxClient.myUserId) { 
			GameObject user = GameObject.Find(objName);
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
			GameObject user = GameObject.Find(objName);
			if (user&&user.GetComponent<NetworkTransformSender>()!=null) {
				user.SendMessage("ForceSendTransform");
				Debug.Log("ForceSendTransform: "+ objName);
			}
		}
	}

}
