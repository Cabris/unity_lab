using UnityEngine;
using System.Collections;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;


public class ClientNetworkController : NetworkController {

	public NetworkTransformReceiver[] propsReceiver; 
	public string sceneName;
	ClientSpawnController spawn;
	public string scene;
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
		spawn=GetComponent<ClientSpawnController>();
		smartFoxClient.JoinRoom("Central Square");
	}

	protected override void OnJoinRoom (Room room)
	{
		base.OnJoinRoom (room);
		Hashtable data = new Hashtable();
		data.Add("sceneName","lab");
		SmartFoxClient client = ClientNetworkController.GetClient();
		client.SendXtMessage("test","clientOnline",data);
	}

	protected override void onExtensionResponse (object obj, string type)
	{
		base.onExtensionResponse (obj, type);
		SFSObject data=obj as SFSObject;
		HandleReceiveData(data);
	}

	protected override void OnUserLeaveRoom (int roomId, int userId, string userName)
	{
		base.OnUserLeaveRoom (roomId, userId, userName);
		spawn.UserLeaveRoom(userId);
		if(userName==scene){//scene lost
			Application.Quit();
		}
	}

	protected override void OnObjectReceived (SFSObject data, User fromUser)
	{
		base.OnObjectReceived (data, fromUser);
		HandleReceiveData(data);
	}

	void HandleReceiveData(SFSObject data){
		string cmd=data.GetString("cmd");
		if(cmd=="t"){
			string object_name=data.GetString("object_name");
			GameObject g=GameObject.Find(object_name);
			if(g!=null){
				NetworkTransformReceiver tr=g.GetComponent<NetworkTransformReceiver>();
				if(tr!=null)
					tr.ReceiveTransform(data);
			}
		}
		if(cmd=="playerStatus"){
			if(data.GetString("userName")==GetClient().myUserName)
				spawn.SpawnLocalPlayer(data);
			else
				spawn.SpawnRemotePlayer(data);
			scene=data.GetString("scene");
		}
	}

	void OnGUI() {
		if(spawn.localPlayer!=null)
		GUI.Label(new Rect(10, 25, 500, 24), 
		          "local player: " + 
		          spawn.localPlayer.name+
		          ", pos: "+spawn.localPlayer.transform.position );
		for(int i=0;i<spawn.remotePlayers.Count;i++){
			GameObject r=spawn.remotePlayers[i];
			GUI.Label(new Rect(10, 50+25*i, 500, 24), 
			          "remote player: " + 
			          r.name+", pos: "+r.transform.position );
		}
	}
	

}
