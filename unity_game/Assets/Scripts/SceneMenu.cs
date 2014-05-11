using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class SceneMenu : NetworkController {
	Dictionary<string,string> sceneInfoMap;
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		smartFoxClient = GetClient();
		if (smartFoxClient==null) {
			Application.LoadLevel("login");
			return;
		}
		sceneInfoMap=new Dictionary<string, string>();
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

	void GoToClient(){
		GetClient().LeaveRoom(GetClient().activeRoomId);
	}

	protected override void HandleReceiveData (SFSObject data)
	{
		base.HandleReceiveData (data);
		string cmd=data.GetString("cmd");
		if(cmd=="SceneInfo"){
			SFSObject dataList=data.Get("dataList") as SFSObject;
			sceneInfoMap.Clear();
			foreach(string k in dataList.Keys()){
				string v=dataList.GetString(k);
				sceneInfoMap.Add(k,v);
				Debug.Log(k);
			}
		}
	}

	void Update ()
	{ 
		//RequestSceneInfo();
	}

	void OnGUI() {

	}
}
