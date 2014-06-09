using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ScenePlayerCommand : MonoBehaviour {
	PlayerAnimation playerAni;
	private Queue queue = new Queue();

	public bool _buttonB=false;
	public GameObject detectObj;	
	public int CommandQueueSize;
	
	public delegate void ButtonEventHandler(string button);
	public ButtonEventHandler OnButtonDown;
	public ButtonEventHandler OnButtonUp;

	// Use this for initialization
	void Start () {
		playerAni=GetComponent<PlayerAnimation>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(queue.Count>0){
			SFSObject data=queue.Dequeue() as SFSObject;
			HandleCommand(data);
		}
		CommandQueueSize=queue.Count;
	}

	public void ReceiveCommand(SFSObject data) {	
		queue.Enqueue(data);
	}

	void HandleCommand(SFSObject data){
		string horizontal=data.GetString("horizontal");
		string vertical=data.GetString("vertical");
		playerAni.direction=Single.Parse( horizontal);
		playerAni.speed=Single.Parse( vertical);
		HandleButton(data,"buttonB",ref _buttonB);
		string detect=data.GetString("detected_object_name");
		detectObj=GameObject.Find(detect);
	}

	void HandleButton(SFSObject data,string button,ref bool preState){
		bool newState=data.GetBool(button);
		if(preState&&!newState&&OnButtonUp!=null)
			OnButtonUp(button);
		if(!preState&&newState&&OnButtonDown!=null)
			OnButtonDown(button);
		preState=newState;
	}
}
