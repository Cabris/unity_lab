using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class AnimationSynchronizer : MonoBehaviour {
	
	private bool sendMode = false;
	private bool receiveMode = false;
	private string lastState = "idle";
			
	// We call it on local player to start sending animation messages
	void StartSending() {
		sendMode = true;
		receiveMode = false;		
	}
	
	// We call it on remote player model to start receiving animation messages
	void StartReceiving() {
		sendMode = false;
		receiveMode = true;
		animation.Play(lastState);
	}

	
	void PlayAnimation(string message) {
		if (sendMode) {
			//if the new state differs, send animation message to other clients
			if (lastState!=message) {
				lastState = message;
				SendAnimationMessage(message);
			}			
		}
		else if (receiveMode) {
			// Just play this animation
			animation.CrossFade(message);	
		}
	}	
	
	void SendAnimationMessage(string message) {
		SmartFoxClient client = NetworkController.GetClient();
		SFSObject data = new SFSObject();
		data.Put("_cmd", "a");  //We put _cmd = "a" here to know that this object contains animation message
		data.Put("mes", message);
		client.SendObject(data);
	}
}
