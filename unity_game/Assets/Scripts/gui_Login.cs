﻿using UnityEngine;
using System;
using System.Collections;			// for using hash tables
using System.Security.Permissions;	// for getting the socket policy
using SmartFoxClientAPI;			// to setup SmartFox connection
using SmartFoxClientAPI.Data;		// necessary to access the room resource

public class gui_Login : MonoBehaviour {
	
	// smartFox variables
	private SmartFoxClient smartFox;
	private string serverIP = "140.115.53.97";
	private int serverPort = 9339;				// default = 9339
	public string zone = "city";
	public bool debug = true;
	public bool isServer=false;
	string targetScene="serverLab";
	// variables used in script
	private string statusMessage = "";
	private string username = "";

	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;// = new ComboBox();
	private GUIStyle listStyle = new GUIStyle();

	void Start()
	{
		comboBoxList = new GUIContent[5];
		comboBoxList[0] = new GUIContent("Thing 1");
		comboBoxList[1] = new GUIContent("Thing 2");
		comboBoxList[2] = new GUIContent("Thing 3");
		comboBoxList[3] = new GUIContent("Thing 4");
		comboBoxList[4] = new GUIContent("Thing 5");
		
		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background =
			listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left =
			listStyle.padding.right =
				listStyle.padding.top =
				listStyle.padding.bottom = 4;
		
		comboBoxControl = new ComboBox(new Rect(100, 120, 100, 20), comboBoxList[0], comboBoxList, "button", "box", listStyle);
	}

	void Awake() {
		Application.runInBackground = true; 	

		if ( SmartFox.IsInitialized() ) {
			Debug.Log("SmartFox is already initialized, reusing connection");
			smartFox = SmartFox.Connection;
		} else {
			if( Application.platform == RuntimePlatform.WindowsWebPlayer ) {
				Security.PrefetchSocketPolicy(serverIP, serverPort);
			}
			try {
				Debug.Log("Starting new SmartFoxClient");
				smartFox = new SmartFoxClient(debug);
				smartFox.runInQueueMode = true;
			} catch ( Exception e ) {
				Debug.Log(e.ToString());
			}
		}
		
		// Register callback delegates, before callling Connect()
		SFSEvent.onConnection += OnConnection;
		SFSEvent.onConnectionLost += OnConnectionLost;
		SFSEvent.onLogin += OnLogin;
		SFSEvent.onRoomListUpdate += OnRoomList;
		SFSEvent.onDebugMessage += OnDebugMessage;
		//SFSEvent.onJoinRoom += OnJoinRoom;		// We will not join a room in this level
		
		Debug.Log("Attempting to connect to SmartFoxServer");
		smartFox.Connect(serverIP, serverPort);		
	}
	
	void FixedUpdate() {
		smartFox.ProcessEventQueue();
	}
	
	void OnGUI() {

		// server IP in bottom left corner
		GUI.Label(new Rect(10, Screen.height-25, 200, 24), "Server: " + serverIP);
		
		// quit button in bottom right corner
		if ( Application.platform != RuntimePlatform.WindowsWebPlayer ) {			
			if ( GUI.Button(new Rect(Screen.width-150, Screen.height - 50, 100, 24), "Quit") ) {
				smartFox.Disconnect();
				UnregisterSFSSceneCallbacks();
				Application.Quit();
			}
		}

		// Show login fields if connected and reconnect button if disconnect
		if (smartFox.IsConnected()) {

			isServer = GUI.Toggle(new Rect(10, 80, 250, 30), isServer, "isServer");

			if(isServer){
				targetScene="serverLab";
				GUI.Label(new Rect(10, 120, 100, 100), "SceneName: ");
				int i=comboBoxControl.Show();
				//Debug.Log(i);
				GUIContent c=comboBoxList[i];
				username=c.text;

			}else{
				targetScene="clientLab";
				GUI.Label(new Rect(10, 120, 100, 100), "Username: ");
				username = GUI.TextField(new Rect(100, 120, 200, 20), username, 25);
			}
			if ( GUI.Button(new Rect(Screen.width/2-150, Screen.height/2, 300, 24), "Login as "+targetScene)  
			    || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
				smartFox.Login(zone, username, "");
			}

		} else {
			if ( GUI.Button(new Rect(Screen.width/2-50, Screen.height/2, 100, 24), "Reconnect")  
			    || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
				Application.LoadLevel("login");
			}
		}
		
		// Draw box for status messages, if one is given
		// Contains some logic to parse message of multiple lines if necessary
		if (statusMessage.Length > 0)
		{
			int boxLength = 61;							// define length of status box
			int messageLength = statusMessage.Length;	// get length of status message
			string originalMessage = statusMessage;		// copy message in to work string
			string formattedMessage = "";				// define output message string
			int i = 0;
			while (i + boxLength < messageLength)		// iterate and add newline until over length
			{
				formattedMessage = formattedMessage + originalMessage.Substring(i,boxLength) + "\n";
				i = i + boxLength;
			}
			// add last piece of original message
			formattedMessage = formattedMessage + originalMessage.Substring(i,  boxLength - (i + boxLength - messageLength));
			// draw status box with message
			GUI.Box (new Rect (Screen.width - 420,10,400,48), formattedMessage);
		}
		
	}
	
	private void UnregisterSFSSceneCallbacks() {
		// This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
		SFSEvent.onConnection -= OnConnection;
		SFSEvent.onConnectionLost -= OnConnectionLost;
		SFSEvent.onLogin -= OnLogin;
		SFSEvent.onRoomListUpdate -= OnRoomList;
		SFSEvent.onDebugMessage -= OnDebugMessage;
		//SFSEvent.onJoinRoom -= OnJoinRoom;
	}
	
	void OnConnection(bool success, string error) {
		if ( success ) {
			SmartFox.Connection = smartFox;
			statusMessage = "Connected to SmartFox Server";
			Debug.Log(statusMessage);
		} else {
			statusMessage = "Can't connect! " + error;
			Debug.Log(statusMessage);
		}
	}
	
	void OnConnectionLost() {
		statusMessage = "Connection lost / no connection to server";
	}
	
	public void OnDebugMessage(string message) {
		//Debug.Log("[SFS DEBUG] " + message);
	}
	
	public void OnLogin(bool success, string name, string error) {
		if ( success ) {
			statusMessage = "Login for user \"" + name +  "\" successful.";
			// Lets wait for the room list
		} else {
			// Login failed - lets display the error message sent to us
			statusMessage = "Login error: " + error;
		}
	}

	void OnApplicationQuit() {
		smartFox.Disconnect();
		UnregisterSFSSceneCallbacks();
	}
	
	void OnRoomList(Hashtable roomList) {
		try {
			foreach (int roomId in roomList.Keys)	{					
				Room room = (Room)roomList[roomId];
				if (room.IsPrivate()) {
					//Debug.Log("Room id: " + roomId + " has name: " + room.GetName() + "(private)");
				}
				//Debug.Log("Room id: " + roomId + " has name: " + room.GetName());
			}
			UnregisterSFSSceneCallbacks();
			Application.LoadLevel(targetScene);
		}
		catch (Exception e) {
			//Debug.Log("Room list error: "+e.Message+" "+e.StackTrace);
		}
	}
}