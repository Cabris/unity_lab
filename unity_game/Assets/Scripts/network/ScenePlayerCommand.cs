using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ScenePlayerCommand : MonoBehaviour {
	PlayerAnimation playerAni;
	private Queue queue = new Queue();
	public int qc;
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
		qc=queue.Count;
	}

	public void ReceiveCommand(SFSObject data) {	
		queue.Enqueue(data);
	}

	void HandleCommand(SFSObject data){
		playerAni.left=data.GetBool("left");
		playerAni.right=data.GetBool("right");
		playerAni.up=data.GetBool("up");
		playerAni.down=data.GetBool("down");
		playerAni.isToSend=true;
	}
}
