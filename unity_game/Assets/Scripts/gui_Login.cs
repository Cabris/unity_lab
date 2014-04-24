using UnityEngine;
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
	 string targetScene="sc_City";
	// variables used in script
	private string statusMessage = "";
	private string username = "";
	
	void Awake() {
		Application.runInBackground = true; 	// Let the application be running while the window is not active.
		
		// Create SmartFox connection if not already available
		if ( SmartFox.IsInitialized() ) {
			Debug.Log("SmartFox is already initialized, reusing connection");
			smartFox = SmartFox.Connection;
		} else {
			if( Application.platform == RuntimePlatform.WindowsWebPlayer ) {
				// Only set this for the webplayer, it breaks pc standalone
				// See http://answers.unity3d.com/questions/25122/ for details
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
		if(isServer){
			targetScene="serverLab";
		}else{
			targetScene="clientLab";
		}
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
			GUI.Label(new Rect(10, 116, 100, 100), "Username: ");
			username = GUI.TextField(new Rect(100, 116, 200, 20), username, 25);
			if ( GUI.Button(new Rect(100, 166, 100, 24), "Login as "+targetScene)  || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
				smartFox.Login(zone, username, "");
			}
		} else {
			if ( GUI.Button(new Rect(100, 166, 100, 24), "Reconnect")  || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
				Application.LoadLevel(targetScene);
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
		Debug.Log("[SFS DEBUG] " + message);
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
	
	/*	
	// We will not join a room in this level, the NetworkController in the next scene will take care of that
	void OnJoinRoom(Room room)
	{
		Debug.Log("Room " + room.GetName() + " joined successfully");
		smartFox.SendPublicMessage(smartFox.myUserName + " has joined");
		// We can now move on to the next level
		UnregisterSFSSceneCallbacks();
		Application.LoadLevel("sc_City");
	}
*/
	
	void OnRoomList(Hashtable roomList) {
		try {
			foreach (int roomId in roomList.Keys)	{					
				Room room = (Room)roomList[roomId];
				if (room.IsPrivate()) {
					Debug.Log("Room id: " + roomId + " has name: " + room.GetName() + "(private)");
				}
				Debug.Log("Room id: " + roomId + " has name: " + room.GetName());
			}
			// Users always have to be in a room, but we'll do that in the next level
			/*
			if (smartFox.GetActiveRoom() == null) {
				smartFox.JoinRoom("Central Square");
			}*/	
			UnregisterSFSSceneCallbacks();
			Application.LoadLevel(targetScene);
		}
		catch (Exception e) {
			Debug.Log("Room list error: "+e.Message+" "+e.StackTrace);
		}
	}
}