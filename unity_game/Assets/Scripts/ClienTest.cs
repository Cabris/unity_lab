using UnityEngine;
using System.Collections;

public class ClienTest : MonoBehaviour {
	bool showDetail,isClient=true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isClient)
		MasterServer.RequestHostList("YourPCRoom");
	}



	void OnGUI() {
		int fontSize=50;
		GUI.skin.label.fontSize = fontSize-20;
		GUI.skin.button.fontSize = fontSize;
		//#if UNITY_EDITOR
		if (GUILayout.Button ("Start Server")) {
			Network.InitializeServer(1, 25002, !Network.HavePublicAddress());
			MasterServer.RegisterHost("YourPCRoom", "PhantasyNan", "alpha 1.0");
			isClient=false;
		}
		GUI.Label(new Rect(Screen.width* 2 /4,10,350,60), "Connections: " +Network.connections.Length);


		//#elif UNITY_ANDROID

//		fontSize=50;
//		GUI.skin.label.fontSize = fontSize;
//		GUI.skin.button.fontSize = fontSize;
		GUILayout.Space(50);
		if (GUILayout.Button("Connect to your Computer"))
			showDetail = !showDetail;
		HostData[] data   = MasterServer.PollHostList();
		foreach (HostData element in data)
		{
			if(showDetail)
			{
				GUILayout.BeginHorizontal();
				var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
				GUILayout.Label(name);
				GUILayout.Space(50);
				string hostInfo;
				hostInfo = "[";
				foreach (string host in element.ip)
					hostInfo = hostInfo + host + ":" + element.port + " ";
				hostInfo = hostInfo + "]";
				GUILayout.Space(5);
				GUILayout.Label(element.comment);
				GUILayout.Space(5);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Connect"))
				{
					Network.Connect(element);
				}
				GUILayout.EndHorizontal();
			}
		}
		//#endif
	}
	
	
	
}
