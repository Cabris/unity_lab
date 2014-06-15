using UnityEngine;
using System;
using System.Collections;			// for using hash tables
using System.Security.Permissions;	// for getting the socket policy
using SmartFoxClientAPI;			// to setup SmartFox connection
using SmartFoxClientAPI.Data;		// necessary to access the room resource

public class ServerConnection:MonoBehaviour
{
	//[SerializeField]
	string serverIP = "140.115.53.97";
	//string serverIP = "127.0.0.1";
	[SerializeField]
	int serverPort = 9339;			
	[SerializeField]
	string zone = "city";

	public SmartFoxClient smartFox;
	public bool isLocalhost=true;
	public string ServerIP{get{return serverIP;}}


	public void Connect(bool debug){
		Application.runInBackground = true; 	
		if(isLocalhost)
			serverIP="127.0.0.1";
		if ( SmartFox.IsInitialized() ) {
			Debug.Log("SmartFox is already initialized, reusing connection");
			smartFox = SmartFox.Connection;
		} else {
			if( Application.platform == RuntimePlatform.WindowsWebPlayer ) {
				Security.PrefetchSocketPolicy(serverIP,serverPort);
			}
			try {
				Debug.Log("Starting new SmartFoxClient");
				smartFox = new SmartFoxClient(debug);
				smartFox.runInQueueMode = true;
			} catch ( Exception e ) {
				Debug.Log(e.ToString());
			}
		}
		
		//		// Register callback delegates, before callling Connect()
		//		SFSEvent.onConnection += OnConnection;
		//		SFSEvent.onConnectionLost += OnConnectionLost;
		//		SFSEvent.onLogin += OnLogin;
		//		SFSEvent.onRoomListUpdate += OnRoomList;
		//		SFSEvent.onDebugMessage += OnDebugMessage;
		//		//SFSEvent.onJoinRoom += OnJoinRoom;		// We will not join a room in this level
		
		Debug.Log("Attempting to connect to SmartFoxServer");
		smartFox.Connect(serverIP, serverPort);		
		
	}
	
	public void Disconnect(){
		smartFox.Disconnect();
	}
	
	public void Login(string username){
		smartFox.Login(zone, username, "");
	}
	
	public bool IsConnected(){
		return smartFox.IsConnected();
	}
	
	public void ProcessEventQueue(){
		smartFox.ProcessEventQueue();
	}
	
}


