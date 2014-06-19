using UnityEngine;
using System;
using System.Collections;			// for using hash tables
using System.Security.Permissions;	// for getting the socket policy
using SmartFoxClientAPI;			// to setup SmartFox connection
using SmartFoxClientAPI.Data;		// necessary to access the room resource

public class gui_Login : MonoBehaviour {
	
	// smartFox variables
	
	public bool debug = true;
	public bool isScene=false;
	
	private string statusMessage = "";
	private string username = "";
	
	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl;// = new ComboBox();
	private GUIStyle listStyle = new GUIStyle();
	private string[] scenes={"Scene0","Scene1","Scene2"};
	
	public ServerConnection serverConnection;
	
	void Start()
	{
		comboBoxList = new GUIContent[scenes.Length];
		for(int i=0;i<scenes.Length;i++){
			comboBoxList[i] = new GUIContent(scenes[i]);
		}
		
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
		RegisterSFSSceneCallbacks();
		LoadGameObject ("ServerConnection");
		//Coroutine co= StartCoroutine(LoadGameObject("ServerConnection"));
		Application.targetFrameRate = 60;
	}

	private void LoadGameObject(string assetName){
		WWW bundle=null;
		string path = string.Format(Extensions.AssetBundleLoaction, assetName);
		Debug.Log("p:"+path);
		try{
			//loading assetbundle
			bundle = new WWW(path);
		}
		catch(Exception e){
			Debug.LogException(e);
		}
		//wait for loaded
		//yield return bundle;
		
		//Instantiate GameObject wait
		GameObject connection=UnityEngine.Object.Instantiate(bundle.assetBundle.mainAsset) as GameObject;
		Debug.Log("loaded: "+path);
		//yield return connection;
		bundle.assetBundle.Unload(false);
		serverConnection = connection.GetComponent<ServerConnection> ();
		serverConnection.Connect(debug);
	}

	
	void FixedUpdate() {
		serverConnection.ProcessEventQueue();
	}
	
	void OnGUI() {
		
		// server IP in bottom left corner
		GUI.Label(new Rect(10, Screen.height-25, 200, 24), "Server: " + serverConnection.ServerIP);
		
		// quit button in bottom right corner
		if ( Application.platform != RuntimePlatform.WindowsWebPlayer ) {			
			if ( GUI.Button(new Rect(Screen.width-150, Screen.height - 50, 100, 24), "Quit") ) {
				serverConnection.Disconnect();
				UnregisterSFSSceneCallbacks();
				Application.Quit();
			}
		}
		
		// Show login fields if connected and reconnect button if disconnect
		if (serverConnection.IsConnected()) {
			
			isScene = GUI.Toggle(new Rect(10, 80, 250, 30), isScene, "isServer");
			string asWhat="";
			if(isScene){
				GUI.Label(new Rect(10, 120, 100, 100), "SceneName: ");
				int i=comboBoxControl.Show();
				//Debug.Log(i);
				GUIContent c=comboBoxList[i];
				SceneController.AssetName =scenes[i];
				string r="r"+Convert.ToInt32(UnityEngine.Random.Range(0,500)).ToString();
				username=c.text+"_"+r;
				asWhat="Scene";
			}else{
				GUI.Label(new Rect(10, 120, 100, 100), "Username: ");
				username = GUI.TextField(new Rect(100, 120, 200, 20), username, 25);
				asWhat="player";
			}
			if ( GUI.Button(new Rect(Screen.width/2-150, Screen.height/2, 300, 24), "Login as "+asWhat)  
			    || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
				serverConnection.Login(username);
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
		SFSEvent.onConnection -= OnConnection;
		SFSEvent.onConnectionLost -= OnConnectionLost;
		SFSEvent.onLogin -= OnLogin;
		SFSEvent.onRoomListUpdate -= OnRoomList;
		SFSEvent.onDebugMessage -= OnDebugMessage;
	}

	private void RegisterSFSSceneCallbacks() {
		SFSEvent.onConnection += OnConnection;
		SFSEvent.onConnectionLost += OnConnectionLost;
		SFSEvent.onLogin += OnLogin;
		SFSEvent.onRoomListUpdate += OnRoomList;
		SFSEvent.onDebugMessage += OnDebugMessage;
	}
	
	void OnConnection(bool success, string error) {
		if ( success ) {
			SmartFox.Connection = serverConnection.smartFox;
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
		serverConnection.Disconnect();
		UnregisterSFSSceneCallbacks();
	}
	
	void OnRoomList(Hashtable roomList) {
		try {
			foreach (int roomId in roomList.Keys)	{					
				Room room = (Room)roomList[roomId];
				//Debug.Log(roomId+","+room.GetName());
				if (room.IsPrivate()) {
				}
			}
			UnregisterSFSSceneCallbacks();
			if(isScene)
				Application.LoadLevel("serverLab");
			else
				Application.LoadLevel("SceneMenu");
		}
		catch (Exception e) {
			Debug.Log("Room list error: "+e.Message+" "+e.StackTrace);
		}
	}
}