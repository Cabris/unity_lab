using UnityEngine;
using System.Collections;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;


public class ClientNetworkController : NetworkController {

	public NetworkTransformReceiver[] propsReceiver; 
	
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

	}

	public static void ForceRemoteObjectToSendTransform (GameObject go)
	{
		SmartFoxClient client = ClientNetworkController.GetClient ();
		SFSObject data = new SFSObject ();
		data.Put ("_cmd", "f");  //We put _cmd = "f" here to know that this object contains "force send transform" demand
		data.Put ("object_name", go.name); // Who this message is for
		data.Put ("to_user", "scene"); // Who this message is for
		client.SendObject (data);
		Debug.Log("ForceRemoteObjectToSendTransform: "+go.name);
	}

	protected  override void OnJoinRoom(Room room) {
		SendMessage("SpawnPlayers");
		Debug.Log("Connected !");
		foreach(NetworkTransformReceiver r in propsReceiver){
			r.StartReceiving();
			//Debug.Log("force t: "+r.gameObject.name);
			ForceRemoteObjectToSendTransform(r.gameObject);
		}
	}
	
	// Here we process incoming SFS objects
	protected  override void OnObjectReceived(SFSObject data, User fromUser) {
		String _cmd = data.GetString("_cmd");
		switch (_cmd) {
			case "t":  // "t" - means transform sync data
				SendTransformToRemoteObject(data, fromUser);
				break;
			case "f":  // "f" - means force our local player to send his transform
				ForceSendTransform(data);
				break;
//			case "a": // "a" - for animation message received
//				SendAnimationMessageToRemotePlayerObject(data, fromUser);
//				break;
		}
	}

	private void SendTransformToRemoteObject(SFSObject data, User fromUser) {
		int fromUserId = fromUser.GetId();
		string objName= data.GetString("object_name");
		if (fromUserId!=smartFoxClient.myUserId) {
			GameObject user = GameObject.Find(objName);
			if (user!=null&&user.GetComponent<NetworkTransformReceiver>()!=null)
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

}
