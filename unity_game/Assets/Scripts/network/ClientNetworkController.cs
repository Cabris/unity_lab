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
		SubscribeEvents();
		started = true;
		spawn=GetComponent<ClientSpawnController>();
		smartFoxClient.JoinRoom(ServerConnection.ConnectionConfig.Room);
		userType = MyUserType.Client;
	}
	
	public static void SendExMsg(string exName,string cmd,Hashtable data){
		//SmartFoxClient client = GetClient();
		data.Add("host",hostSceneName);
//		if(client!=null)
//			client.SendXtMessage(exName,cmd,data);
		NetworkController.SendExMsg (exName, cmd, data);
	}
	
	protected override void OnJoinRoom (Room room)
	{
		base.OnJoinRoom (room);
		Hashtable data = new Hashtable();
		SendExMsg("test","joinScene",data);
	}
	
	protected override void OnUserLeaveRoom (int roomId, int userId, string userName)
	{
		base.OnUserLeaveRoom (roomId, userId, userName);
		spawn.UserLeaveRoom(userId);
		if(userName==hostSceneName){//scene lost
			UnsubscribeEvents();
			Application.Quit();
		}
	}
	
	protected override void HandleReceiveData(SFSObject data){
		base.HandleReceiveData(data);
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
		if(cmd=="playerStatus"){
			if(data.GetString("userName")==GetClient().myUserName)
				spawn.SpawnLocalPlayer(data);
			else
				spawn.SpawnRemotePlayer(data);
		}
		if(cmd=="sceneData"){
			SFSObject datas =data.Get("datas") as SFSObject;
			foreach(object obj in datas.Keys()){
				SFSObject sdata=datas.Get(obj) as SFSObject;
				clientController.HandleSceneObject(sdata);
			}
		}
		if(cmd=="sceneObject"){
			string object_name=data.GetString("object_name");
			GameObject g=GameObject.Find(object_name);
			if(g!=null){
				SceneObject s=g.GetComponent<SceneObject>();
				if(s!=null){
					s.ReceiveMessage(data);
				}
			}
		}
		if(cmd=="mechinePart"){
			string object_name=data.GetString("object_name");
			GameObject g=GameObject.Find(object_name);
			if(g!=null){
				MechinePart m=g.GetComponent<MechinePart>();
				if(m!=null){
					m.ReceiveMessage(data);
				}
			}
		}
		if(cmd=="a"){
			string object_name=data.GetString("object_name");
			GameObject g=GameObject.Find(object_name);
			if(g!=null){
				PlayerAnimation a=g.GetComponent<PlayerAnimation>();
				if(a!=null){
					a.ReceiveAni(data);
				}
			}
		}
	}
	
	void OnGUI() {
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
	
//	int i=0;
	void Update ()
	{ 
//		if (Input.GetKey(KeyCode.B)) {
//			SmartFoxClient client = ClientNetworkController.GetClient ();
//			SFSObject data = new SFSObject ();
//			data.Put ("cmd", "#");
//			data.Put ("#", "i="+i);
//			client.SendObject (data);
//			i++;
//		} 
	}
}
