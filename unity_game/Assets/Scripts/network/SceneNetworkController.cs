using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class SceneNetworkController : NetworkController
{
	
	public SceneController sceneController;
	SceneSpawnController spawn;
	// We start working from here
	void Start ()
	{
		Application.runInBackground = true; // Let the application be running whyle the window is not active.
		smartFoxClient = GetClient ();
		if (smartFoxClient == null) {
			Application.LoadLevel ("login");
			return;
		}
		SubscribeEvents ();
		started = true;
		spawn = GetComponent<SceneSpawnController> ();
		
		smartFoxClient.JoinRoom ("Central Square");
	}
	
	protected void RegistScene ()
	{
		Hashtable data = new Hashtable ();
		data.Add ("type", Application.loadedLevelName);
		SmartFoxClient client = ClientNetworkController.GetClient ();
		client.SendXtMessage ("test", "registScene", data);
	}
	
	protected override void OnJoinRoom (Room room)
	{
		base.OnJoinRoom (room);
		RegistScene ();
	}
	
	protected override void OnUserEnterRoom (int roomId, User user)
	{
		base.OnUserEnterRoom (roomId, user);
		if (roomId == GetClient ().activeRoomId) {
			spawn.SpawnServerPlayer (user);
			SendSceneData (user);
		}
	}
	
	protected override void HandleReceiveData (SFSObject data)
	{
		string cmd = data.GetString ("cmd");
		if (cmd == "m") {
			string object_name = data.GetString ("object_name");
			GameObject g = GameObject.Find (object_name);
			ScenePlayerCommand c = g.GetComponent<ScenePlayerCommand> ();
			c.ReceiveCommand (data);
		}
		if (cmd == "#") {
			string msg = data.GetString ("#");
			Debug.Log (msg);
		}
	}
	
	protected override void OnUserLeaveRoom (int roomId, int userId, string userName)
	{
		base.OnUserLeaveRoom (roomId, userId, userName);
		if (roomId == GetClient ().activeRoomId) {
			spawn.UserLeaveRoom (userId);
		}
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (10, 25, 500, 24), 
		           "scene: " + GetClient ().myUserName);
		int i = 0;
		foreach (User u in spawn.scenePlayers.Keys) {
			GameObject s = spawn.scenePlayers [u];
			GUI.Label (new Rect (10, 60 + 25 * i, 500, 24), 
			           "" + (i + 1) + ". user: " + u.GetName () +
			           ", scene player: " + 
			           s.name + ", pos: " + s.transform.position);
			i++;
		}
	}
	
	void SendSceneData (User toUser)
	{
		SceneObject[] sceneObjs = sceneController.GetComponentsInChildren<SceneObject> ();
		SmartFoxClient client = GetClient ();
		SFSObject data = new SFSObject ();
		data.Put ("cmd", "sceneData");
		data.Put ("toUser", toUser.GetName ());
		List<SFSObject> sdatas = new List<SFSObject> ();
		foreach (SceneObject s in sceneObjs) {
			SFSObject sdata = s.GetData ();
			sdatas.Add (sdata);
		}
		data.PutList ("datas", sdatas);
		client.SendObject (data);
	}
	
}
