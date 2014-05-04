using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class SceneNetworkController : NetworkController {
	
	public SceneController sceneController;
	SceneSpawnController spawn;
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
		spawn=GetComponent<SceneSpawnController>();
		smartFoxClient.JoinRoom("Central Square");
	}

	protected override void OnJoinRoom (Room room)
	{
		base.OnJoinRoom (room);
		Hashtable data = new Hashtable();
		data.Add("sceneName","lab");
		SmartFoxClient client = ClientNetworkController.GetClient();
		client.SendXtMessage("test","sceneOnline",data);
	}

	protected override void OnUserEnterRoom (int roomId, User user)
	{
		base.OnUserEnterRoom (roomId, user);
		spawn.SpawnServerPlayer(user);
		SendSceneData(user);
	}

	protected override void OnObjectReceived (SFSObject data, User fromUser)
	{
		base.OnObjectReceived (data, fromUser);
		HandleReceiveData(data);
	}

	void HandleReceiveData(SFSObject data){
		string cmd=data.GetString("cmd");
		if(cmd=="m"){
			string object_name=data.GetString("object_name");
			GameObject g=GameObject.Find(object_name);
			ScenePlayerCommand c=g.GetComponent<ScenePlayerCommand>();
			c.ReceiveCommand(data);
		}
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
	}

	void OnGUI() {
		int i=0;
		foreach (User u in spawn.scenePlayers.Keys){
			GameObject s=spawn.scenePlayers[u];
			GUI.Label(new Rect(10, 25+25*i, 500, 24), 
			          "user: "+u.GetName()+
			          ", scene player: " + 
			          s.name+", pos: "+s.transform.position );
				i++;
		}
	}

	void SendSceneData(User toUser){
		SceneObject[] sceneObjs=sceneController.GetComponentsInChildren<SceneObject>();
		SmartFoxClient client = GetClient();
		SFSObject data=new SFSObject();
		data.Put("cmd","sceneData");
		data.Put("toUser",toUser.GetName());
		List<SFSObject> sdatas=new List<SFSObject>();
		foreach(SceneObject s in sceneObjs){
			SFSObject sdata=s.GetData();
			sdatas.Add(sdata);
		}
		data.PutList("datas",sdatas);
		client.SendObject(data);
	}

}
