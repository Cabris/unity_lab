﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class SceneMenu : NetworkController {
	Dictionary<string,string> sceneInfoMap=new Dictionary<string, string>();
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		smartFoxClient = GetClient();
		if (smartFoxClient==null) {
			Application.LoadLevel("login");
			return;
		}
		//sceneInfoMap=new Dictionary<string, string>();
		SubscribeEvents();
		started = true;
		smartFoxClient.JoinRoom("The Station");
	}
	
	protected override void OnJoinRoom (Room room)
	{
		base.OnJoinRoom (room);
		RequestSceneInfo();
	}
	
	void RequestSceneInfo(){
		if(smartFoxClient!=null){
			Hashtable data = new Hashtable();
			data.Add("from",GetClient().myUserName);
			GetClient().SendXtMessage("test","requestSceneInfo",data);
		}
	}
	
	void GoToClient(string targetScene){
		//GetClient().LeaveRoom(GetClient().activeRoomId);
		ClientNetworkController.hostSceneName=targetScene;
		Application.LoadLevel("clientLab");
	}
	
	protected override void HandleReceiveData (SFSObject data)
	{
		base.HandleReceiveData (data);
		string cmd=data.GetString("cmd");
		if(cmd=="SceneInfo"){
			SFSObject dataList=data.Get("dataList") as SFSObject;
			sceneInfoMap.Clear();
			foreach(string sceneName in dataList.Keys()){
				string sceneType=dataList.GetString(sceneName);
				sceneInfoMap.Add(sceneName,sceneType);
//				Debug.Log(sceneName);
				
//				SmartFoxClient client = GetClient();
//				Hashtable data2=new Hashtable();
//				data2.Add("host",sceneName);
//				client.SendXtMessage("test","joinScene",data2);
			}
		}
	}
	
	void Update ()
	{ 
		//RequestSceneInfo();
	}
	
	void OnGUI() {
		int i=0;
		foreach(string sceneName in sceneInfoMap.Keys){
			if ( GUI.Button(new Rect(50, 50+i*80, 150, 24), "go to "+sceneName)){
				GoToClient(sceneName);
			}
			i++;
		}
	}
}