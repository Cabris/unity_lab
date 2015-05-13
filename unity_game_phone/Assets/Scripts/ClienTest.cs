using UnityEngine;
using System.Collections;

public class ClienTest : MonoBehaviour {
	//[SerializeField]
	public bool isServer=false;
	
	bool showDetail,isConnected=false;
	// Use this for initialization
	void Start () {
		if(isServer){
		Network.InitializeServer(1, 25002, !Network.HavePublicAddress());
		MasterServer.RegisterHost("YourPCRoom", "PhantasyNan", "alpha 1.0");
		Debug.Log("server started.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Update");
		if(isServer){
			//Debug.Log("isServer");
		}else{
			MasterServer.RequestHostList("YourPCRoom");
			HostData[] data   = MasterServer.PollHostList();
			foreach (HostData element in data)
			{
				//if(showDetail)
				{

					var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;

					string hostInfo;
					hostInfo = "[";
					foreach (string host in element.ip)
						hostInfo = hostInfo + host + ":" + element.port + " ";
					hostInfo = hostInfo + "]";
					Debug.Log("hostInfo: "+hostInfo);
					if (!isConnected)
					{
						Network.Connect(element);
						isConnected=true;
						Debug.Log("Connect");
						break;
					}

				}
			}
		}
	}
	
	
	
	void OnGUI() {
		
//		if(isServer){
//			int fontSize=50;
//			GUI.skin.label.fontSize = fontSize-20;
//			GUI.skin.button.fontSize = fontSize;
//			//#if UNITY_EDITOR
//			if (GUILayout.Button ("Start Server")) {
//				Network.InitializeServer(1, 25002, !Network.HavePublicAddress());
//				MasterServer.RegisterHost("YourPCRoom", "PhantasyNan", "alpha 1.0");
//				Debug.Log("server started.");
//			}
			GUI.Label(new Rect(Screen.width* 2 /4,10,350,60), "Connections: " +Network.connections.Length);
//			
//		}else{
//			
//			//#elif UNITY_ANDROID
//			
//			//		fontSize=50;
//			//		GUI.skin.label.fontSize = fontSize;
//			//		GUI.skin.button.fontSize = fontSize;
//			GUILayout.Space(50);
//			//if (GUILayout.Button("Connect to your Computer"))
//			//	showDetail = !showDetail;
//			HostData[] data   = MasterServer.PollHostList();
//			foreach (HostData element in data)
//			{
//				//if(showDetail)
//				{
//					GUILayout.BeginHorizontal();
//					var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
//					GUILayout.Label(name);
//					GUILayout.Space(50);
//					string hostInfo;
//					hostInfo = "[";
//					foreach (string host in element.ip)
//						hostInfo = hostInfo + host + ":" + element.port + " ";
//					hostInfo = hostInfo + "]";
//					GUILayout.Space(5);
//					GUILayout.Label(element.comment);
//					GUILayout.Space(5);
//					GUILayout.FlexibleSpace();
//					if (!isConnected)
//					{
//						Network.Connect(element);
//						isConnected=true;
//						break;
//					}
//					GUILayout.EndHorizontal();
//				}
//			}
//		}
		//#endif
	}
	
	
	
}
