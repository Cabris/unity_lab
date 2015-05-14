using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO; using System.Text;

public class StagesStatistic : MonoBehaviour {
	public List<StageStatistic> statistics;
	
	
	// Use this for initialization
	void Start () {
		
	}
	bool isSave=false;
	// Update is called once per frame
	void Update () {
		string playerId = Extensions.Player.playerId;
		
		
		#if UNITY_ANDROID
		
		if(!isSave&&Vector3.Distance(Extensions.Player.transform.position,transform.position)<25/2);
		{
			string d="";
			foreach(StageStatistic s in statistics){
				d+=(s.gameObject.name+", "+s.counter.ToString()+"\n");
			}
			networkView.RPC("writeData",RPCMode.OthersBuffered,playerId,d);
			isSave=true;
		}

		#endif
		
		
	}
	
	void OnApplicationQuit() {
		string playerId = Extensions.Player.playerId;
		#if UNITY_EDITOR
		
		if(Extensions.Player.controlType==PlayerBeheaver.ControlType.mono){
			
			FileInfo f = new FileInfo( Application.persistentDataPath + "\\" + playerId+".txt");
			StreamWriter w;
			if(!f.Exists)
			{
				w = f.CreateText();    
			}
			else
			{
				f.Delete();
				w = f.CreateText();
			}
			//w.WriteLine(loadMessage);
			w.WriteLine (playerId);
			foreach(StageStatistic s in statistics){
				w.WriteLine(s.gameObject.name+", "+s.counter.ToString());
			}
			
			w.Close();
			Debug.Log("saves");
		}
		#endif
		
		
	}
	
	
	
	[RPC]
	void writeData(string playerId,string data) {
		//string playerId = Extensions.Player.playerId;
		FileInfo f = new FileInfo( Application.persistentDataPath + "\\" + playerId+".txt");
		StreamWriter w;
		if(!f.Exists)
		{
			w = f.CreateText();    
		}
		else
		{
			f.Delete();
			w = f.CreateText();
		}
		//w.WriteLine(loadMessage);
		w.WriteLine (playerId);
		foreach(StageStatistic s in statistics){
			w.WriteLine(s.gameObject.name+", "+s.counter.ToString());
		}
		
		w.Close();
		Debug.Log("savesRPC");
	}
	
	
}
