using UnityEngine;
using System.Collections;
using System;

public class NetworkTransformSender : MonoBehaviour {

	public float sendingPeriod = 0.1f; // We will send transform each 0.1 second. To make transform synchronization smoother consider writing interpolation algorithm instead of making smaller period.
	private float timeLastSending = 0.0f;

	private bool sendMode = false;
	private NetworkTransform lastState; // Last received and last sent transform state
	
	void Start() {
		lastState = new NetworkTransform(this.gameObject);
	}
		
	// We call it on local player to start sending his transform
	void StartSending() {
		sendMode = true;
	}
	
	void FixedUpdate() { 
		if (sendMode) {
			SendTransform();
		}
	}
	
	void SendTransform() {
		if (lastState.UpdateIfDifferent()) {
				if (timeLastSending >= sendingPeriod) {
					lastState.DoSend();
					timeLastSending = 0;
					return;
				}
		}
		timeLastSending += Time.deltaTime;
	}
	
	//This method is called when remote user forces us to send our transform
	void ForceSendTransform() {
		lastState.DoSend();
	}		
}
