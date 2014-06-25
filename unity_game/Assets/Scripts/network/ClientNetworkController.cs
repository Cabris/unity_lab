using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;


public class ClientNetworkController : NetworkController {
	
	public string sceneName;
	public ClientController clientController;
	//public string scene;
	ClientSpawnController spawn;
	public static string hostSceneName="test";
	
	// We start working from here
	void Start() {
		Application.runInBackground = true; // Let the application be running whyle the window is not active.
		smartFoxClient = GetClient();
		if (smartFoxClient==null) {
			Application.LoadLevel("login");
			return;
		}	
		if (hostSceneName.Length==0) {
			Application.LoadLevel("SceneMenu");
			return;
		}
	//	Debug.Log("1");
		SubscribeEvents();
		started = true;
		spawn=GetComponent<ClientSpawnController>();
		smartFoxClient.JoinRoom(ServerConnection.ConnectionConfig.Room);
		userType = MyUserType.Client;
	//	Debug.Log("2");
	}
	
	public static void SendExMsg(string exName,string cmd,Hashtable data){
		data.Add("host",hostSceneName);
		NetworkController.SendExMsg (exName, cmd, data);
	}
	
	protected override void OnJoinRoom (Room room)
	{
	//	Debug.Log("5");
		base.OnJoinRoom (room);
		Hashtable data = new Hashtable();
		SendExMsg("test","joinScene",data);
	}
	
	protected override void OnUserLeaveRoom (int roomId, int userId, string userName)
	{
	//	Debug.Log("4");
		base.OnUserLeaveRoom (roomId, userId, userName);
		spawn.UserLeaveRoom(userId);
		if(userName==hostSceneName){//scene lost
			UnsubscribeEvents();
			Application.Quit();
		}
	}
	
	protected override void HandleReceiveData(SFSObject data){
//		Debug.Log("6");
		string cmd=data.GetString("cmd");
		if(cmd=="t"){
			string object_name=data.GetString("object_name");
			GameObject g=GameObject.Find(object_name);
			if(g!=null){
				NetworkTransformReceiver tr=g.GetComponent<NetworkTransformReceiver>();
				if(tr!=null){
					tr.ReceiveTransform(data);
				}
			}
		}
		else if(cmd=="playerStatus"){
			if(data.GetString("userName")==GetClient().myUserName)
				spawn.SpawnLocalPlayer(data);
			else
				spawn.SpawnRemotePlayer(data);
		}
		else if(cmd=="sceneData"){
			SFSObject datas =data.Get("datas") as SFSObject;
			foreach(object obj in datas.Keys()){
				SFSObject sdata=datas.Get(obj) as SFSObject;
				clientController.HandleSceneObject(sdata);
			}
		}
		else{
			string object_name=data.GetString("object_name");
			string type=data.GetString("type");
			GameObject g=GameObject.Find(object_name);
			if(g!=null){
				NetworkObject networkObj=g.GetComponent(type) as NetworkObject;
				networkObj.ReceiveMessage(data);
			}
		}

	}
	
	void OnGUI() {
	//	Debug.Log("3");
		if(spawn.localPlayer!=null){
			NetworkTransformReceiver r=spawn.localPlayer.GetComponent<NetworkTransformReceiver>();
			GUI.Label(new Rect(10, 25, 500, 24), 
			          "local player: " + 
			          spawn.localPlayer.name+
			          ", pos: "+spawn.localPlayer.transform.position+"qc: " +r.qc);
		}
		for(int i=0;i<spawn.remotePlayers.Count;i++){
			GameObject r=spawn.remotePlayers[i];
			GUI.Label(new Rect(10, 50+25*i, 500, 24), 
			          "remote player: " + 
			          r.name+", pos: "+r.transform.position );
		}
	}
	

}
