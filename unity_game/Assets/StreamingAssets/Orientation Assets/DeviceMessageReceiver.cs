using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;

public class DeviceMessageReceiver : MonoBehaviour {

	PlayerBeheaver player;
	const string exit_code="ORIENT_EXIT";
	private TcpListener tcpListener;
	private Thread listenThread;
	List<TcpClient> clients=new List<TcpClient>();
	public delegate void OnClientMessage(string msg);
	public OnClientMessage onClientMessage;
	[SerializeField]
	Vector4 ort;
	public Transform target;
	ConcurrentStack<Vector4> orientationStack=new ConcurrentStack<Vector4>();
	[SerializeField]
	EncodeCameraV2 encodeCam;
	bool isClose=false;
	bool HandleClientFlag=true;
	bool isConnected=false;
	
	void Start () {
		player = GameObject.Find ("SceneLogic/local_player_standard").GetComponent<PlayerBeheaver>();
		int port=StreamTcpServer.port2;
		this.tcpListener = new TcpListener(IPAddress.Any, port);
		this.listenThread = new Thread(new ThreadStart(ListenForClients));
		if(Extensions.Player.controlType==PlayerBeheaver.ControlType.stereo)
		this.listenThread.Start();
		onClientMessage+=clientMsg;
	}
	
	
	// Update is called once per frame
	void Update () {
		if(isClose){
			encodeCam.stopEncoding();
			isClose=false;
		}

		if(isConnected){
			player.onConnected();
			isConnected=false;
		}
	}

	Vector4 smoothedValue=Vector4.zero ;
	float smoothing=5;

	//[SerializeField]
	int qSize;

	void FixedUpdate () {
		qSize = orientationStack.Count;
		try {
			if (qSize > 0) {
				Camera.main.depth=-10;
				ort = orientationStack.Pop ();
				smoothedValue += (ort - smoothedValue) / smoothing;
				Quaternion q=new Quaternion(ort.x,ort.y,ort.z,ort.w);

				//Quaternion q = Quaternion.Euler (-ort.w,-ort.x, -ort.y);
				target.localRotation = q;
				target.Rotate(Vector3.back, 90, Space.Self);
				target.Rotate(Vector3.up, 180, Space.World);
				target.Rotate(Vector3.left, 90, Space.World);
				Quaternion q2=Quaternion.LookRotation(target.forward,-target.up);
				target.localRotation = q2;
				//target.Rotate(new Vector3(90,0,0));

				orientationStack.Clear ();
			}
		} catch (Exception e) {
			//Debug.LogException(e);
		}
	}


	
	void clientMsg(string msg){
		string[] values= msg.Split(',');
		float w=Convert.ToSingle( values[0]);
		float x=Convert.ToSingle( values[1]);
		float y=Convert.ToSingle( values[2]);
		float z=Convert.ToSingle( values[3]);
		Vector4 orientation=new Vector4(x,y,z,w);
		orientationStack.Push(orientation);
		if (orientationStack.Count > 2)
			orientationStack.Clear();
	}
	
	void ListenForClients()
	{
		Debug.Log("DeviceMessageReceiver ListenForClients");
		this.tcpListener.Start();
		string ip=((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString();
		//Debug.Log("ListenForClients1: "+ip);
		string sHostName = Dns.GetHostName (); 
		IPHostEntry ipE = Dns.GetHostByName (sHostName); 
		IPAddress [] IpA = ipE.AddressList; 
		for (int i = 0; i < IpA.Length; i++) 
		{ 
			string s= String.Format("IP Address {0}: {1} ", i, IpA[i].ToString ());
			Debug.Log(s);
		}
		
		//while (true){
		//blocks until a client has connected to the server
		TcpClient client = this.tcpListener.AcceptTcpClient();
		Debug.Log("DeviceMessageReceiver client:"+clients.Count);
		ParameterizedThreadStart tStart=new ParameterizedThreadStart(HandleClient);
		HandleClient(client);
		//break;
		//Debug.Log(" DeviceMessageReceiver client end");
		//}
		
	}
	
	private void HandleClient(object client)
	{
		isConnected = true;
		TcpClient tcpClient = (TcpClient)client;
		tcpClient.NoDelay=true;
		tcpClient.ReceiveBufferSize=6000000;
		clients.Add(tcpClient);
		StreamReader reader=new StreamReader(tcpClient.GetStream());
		try{
			while(HandleClientFlag){
				string msg= reader.ReadLine();
				if(msg!=null){
					if(msg==exit_code){
						Debug.Log("orient discont");
						isClose=true;
						HandleClientFlag=false;
						break;
					}
					if(onClientMessage!=null)
						onClientMessage(msg);
				}
			}
		}catch(Exception e){
			Debug.LogException(e);
		}
		
	}
	
	public void onDestory(){
		foreach(TcpClient c in clients)
			c.Close();
		tcpListener.Stop();
		HandleClientFlag=false;
		if(listenThread.IsAlive)
			listenThread.Join();
		//listenThread.Abort();
	}
	
	void OnApplicationQuit() {
		onDestory();	
	}
}
