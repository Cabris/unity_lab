using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ScenePlayerCommand : MonoBehaviour {
	PlayerInput input;
	public bool isWalking;
	public bool isTurnRight=false;
	public bool isTurnLeft=false;
	private Queue queue = new Queue();
	public int qc;
	// Use this for initialization
	void Start () {
		input=GetComponent<PlayerInput>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(queue.Count>0){
			SFSObject data=queue.Dequeue() as SFSObject;
			HandleCommand(data);
		}
		if(isWalking){
			input.StartWalk();
		}
		else{
			input.EndWalk();
		}
		
		if(isTurnLeft){
			input.StartTurn("left");
		}
		else if(isTurnRight){
			input.StartTurn("right");
		}
		else{
			input.EndTurn("left");
			input.EndTurn("right");
		}
		qc=queue.Count;
	}

	public void ReceiveCommand(SFSObject data) {	
		queue.Enqueue(data);
	}

	void HandleCommand(SFSObject data){
		isWalking=data.GetBool("isWalking");
		isTurnLeft=data.GetBool("isTurnLeft");
		isTurnRight=data.GetBool("isTurnRight");
	}
}
