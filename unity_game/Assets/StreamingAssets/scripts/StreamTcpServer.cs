using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;


public class StreamTcpServer : MonoBehaviour {
	
	private TcpListener tcpListener;
	private Thread listenThread;
	List<TcpClient> clients=new List<TcpClient>();
	List<BufferedStream> bStreams=new List<BufferedStream>();
	bool isListening=true;
	List<string> pathes=new List<string>();

	// Use this for initialization
	void Start () {
		this.tcpListener = new TcpListener(IPAddress.Any, 8888);
		this.listenThread = new Thread(new ThreadStart(ListenForClients));
		this.listenThread.Start();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (string p in pathes) {
			QRcodeCreater q=GetComponent<QRcodeCreater> ();
			if(q!=null)
						q.CreateCode (p);
		}
		pathes.Clear();
	}

	void ListenForClients()
	{
		//Debug.Log("ListenForClients0");
		this.tcpListener.Start();
		string ip=((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString();
		//Debug.Log("ListenForClients1: "+ip);
		string sHostName = Dns.GetHostName (); 
		IPHostEntry ipE = Dns.GetHostByName (sHostName); 
		IPAddress [] IpA = ipE.AddressList; 
		for (int i = 0; i < IpA.Length; i++) 
		{ 
			string path=IpA[i].ToString ()+":8888";
			string s= String.Format("IP Address [{0}] {1} ", i, path);
			Debug.Log(s);
			pathes.Add(path);
		}

		while (isListening){
			//blocks until a client has connected to the server
			TcpClient client = this.tcpListener.AcceptTcpClient();
			Debug.Log("StreamTcpServer client:"+clients.Count);
			ParameterizedThreadStart tStart=new ParameterizedThreadStart(HandleClientComm);
			//Thread clientThread=new Thread(tStart);
			//clientThread.Start(client);
			HandleClientComm(client);
			break;
			//Debug.Log("client");
		}
		
	}

	private void HandleClientComm(object client)
	{
		TcpClient tcpClient = (TcpClient)client;
		//tcpClient.NoDelay=true;
		tcpClient.SendBufferSize=600000;
		clients.Add(tcpClient);
		BufferedStream bs=new BufferedStream(tcpClient.GetStream());
		bStreams.Add(bs);
	}
	
	public int Send(byte [] data){
		int length=-1;
		for(int i=0;i<bStreams.Count;i++){
			BufferedStream bs=bStreams[i];
			length=data.Length;
			byte[] lengthData=getBytes(length);
			bs.Write(lengthData, 0 , lengthData.Length); 
			bs.Write(data, 0 , data.Length);   
			bs.Flush();
		}          
		return length;
	}

	byte[] getBytes(int x) {
		return BitConverter2.getBytes(x);
	}

	public void onDestory(){
		isListening=false;
		foreach(BufferedStream bs in bStreams)
			bs.Close();
		bStreams.Clear();
		foreach(TcpClient c in clients)
			c.Close();
		clients.Clear();
		tcpListener.Stop();
		listenThread.Join();
	}

	void OnApplicationQuit() {
		if(isListening)
			onDestory();
	}
}