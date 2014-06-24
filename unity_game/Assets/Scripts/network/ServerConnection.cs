using UnityEngine;
using System;
using System.Collections;			// for using hash tables
using System.Security.Permissions;	// for getting the socket policy
using SmartFoxClientAPI;			// to setup SmartFox connection
using SmartFoxClientAPI.Data;		// necessary to access the room resource
using System.Xml.Serialization;

public class ServerConnection:MonoBehaviour
{
	string serverIP = "140.115.53.97";
	int serverPort = 9339;			
	string zone = "city";

	public SmartFoxClient smartFox;

	public string ServerIP{get{return serverIP;}}
	public static ConnectionConfig ConnectionConfig{get;private set;}

	public void Connect(bool debug){
		loadConfig();
				serverIP=ConnectionConfig.ServerIP;
				serverPort=ConnectionConfig.ServerPort;
				zone=ConnectionConfig.Zone;

		Application.runInBackground = true; 	
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

		Debug.Log("Attempting to connect to SmartFoxServer");
		smartFox.Connect(serverIP, serverPort);		
		
	}

	void loadConfig(){
		string path=@"C:\_AssetBundles\config.xml";

		System.Xml.Serialization.XmlSerializer reader = 
			new System.Xml.Serialization.XmlSerializer(typeof(ConnectionConfig));
		System.IO.StreamReader file = new System.IO.StreamReader(path);

		ConnectionConfig config=new ConnectionConfig();
		config = (ConnectionConfig)reader.Deserialize(file);
		ConnectionConfig=config;
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


