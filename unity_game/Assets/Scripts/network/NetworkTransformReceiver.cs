using UnityEngine;
using System.Collections;
using System;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class NetworkTransformReceiver : MonoBehaviour {
	
	public float yAdjust = 0.0f; // Ajust y position when synchronizing the local and remote models.
	public float interpolationPeriod = 0.01f;  // This value should be equal to the sendingPerion value of the Sender script
	public int qc;
	public long ts=0;

	private bool receiveMode = false;
	private NetworkTransform lastState; // Last received transform state
	private NetworkTransform interpolateTo = null;  // Last state we interpolate to in receiving mode.
	private NetworkTransform interpolateFrom;  // Point from which to start interpolation
	
	private int interpolationPoint = 0; // Current interpolation point
	private int maxInterpolationPoints = 0; // Maximum number of interpolation points;
	private float interpolationDelta = 0; // Delta value by which interpolate
	
	private FPSStorage fpsStorage;
	private Queue queue = new Queue();  // Queue to store transform states for interpolations
	
	// We call it on remote player to start receiving his transform
	public void StartReceiving() {
		lastState = new NetworkTransform(this.gameObject);	
		fpsStorage = GameObject.Find("FPS").GetComponent(typeof(FPSStorage)) as FPSStorage;
		receiveMode = true;
	}
	
	void Update() {
		if (receiveMode) {
			InterpolateTransform();
		}
		qc=queue.Count;
	}
	
	//This method is called when receiving remote transform
	// We update lastState here to know last received transform state
	public void ReceiveTransform(SFSObject data) {
		if (receiveMode) {
			Vector3 pos = GetPos(data);
			pos.y+=yAdjust;
			Quaternion rot = GetRot(data);
			Vector3 sca=GetScale(data);
//			Debug.Log("ReceiveTransform:"+gameObject.name);
			lastState.InitFromValues(pos, rot,sca);
			// Adding next received state to the queue	
			NetworkTransform nextState = new NetworkTransform(this.gameObject);
			nextState.InitFromValues(pos, rot,sca);
			queue.Enqueue(nextState);
		}
	}
	
	// This method is called in every Fixed Update in receiving mode. And it does transform interpolation to the latest state.
	void InterpolateTransform() {
		// If interpolationg
		if (interpolationPoint < maxInterpolationPoints) {
			interpolationPoint++;
			float t = interpolationPoint*interpolationDelta;
			if (t>1) 
				t=1;
			transform.position = Vector3.Lerp(interpolateFrom.position, interpolateTo.position, t);
			transform.rotation = Quaternion.Slerp(interpolateFrom.rotation, interpolateTo.rotation, t);
			transform.localScale = Vector3.Lerp(interpolateFrom.scale, interpolateTo.scale, t);
		}
		else {
			// Finished interpolating to the next point
			if (interpolateTo!=null) {
				// Fixing interpolation result to set transform right to the next point
				//transform.position = interpolateTo.position;
				//transform.rotation = interpolateTo.rotation;
				SetTransform(transform,interpolateTo);
			}
			
			// Take new value from queue
			if (queue.Count!=0) {
				NetworkTransform nextTransform = queue.Dequeue() as NetworkTransform;
				//Start interpolation to the next transform
				// Set new final interpolation state and reset interpolationPoint
				interpolateTo = nextTransform;
				// Set new point from which to start interpolation as current transform
				interpolateFrom = new NetworkTransform(this.gameObject);
				
				interpolationPoint = 0;
				float frameRate = fpsStorage.GetCurrentFPS();
				
				// Calculate the total number of interpolation points as number of frames during interpolationPriod
				maxInterpolationPoints = Convert.ToInt32(Math.Round(frameRate * interpolationPeriod));
				
				// Reset interpolation deltaTime
				interpolationDelta = 1.0f / Convert.ToSingle(maxInterpolationPoints);
			}
			else {
				// If queue is empty just setting the transform to the last received state
				//transform.position = lastState.position;
				//transform.rotation = lastState.rotation;
				SetTransform(transform,lastState);
			}
		}	
	}

	public static void SetTransform(Transform t, SFSObject data){
		t.position=GetPos(data);
		t.rotation=GetRot(data);
		t.localScale=GetScale(data);
	}

	public static void SetTransform(Transform t, NetworkTransform nt){
		t.position=nt.position;
		t.rotation=nt.rotation;
		t.localScale=nt.scale;
	}
	
	public  static Vector3 GetPos(SFSObject data){
		Vector3 pos = new Vector3(Convert.ToSingle(data.GetNumber("x")), 
		                          Convert.ToSingle(data.GetNumber("y")),
		                          Convert.ToSingle(data.GetNumber("z"))
		                          );
		return pos;
	}
	
	public  static Vector3 GetScale(SFSObject data){
		Vector3 sca = new Vector3(Convert.ToSingle(data.GetNumber("sx")), 
		                          Convert.ToSingle(data.GetNumber("sy")),
		                          Convert.ToSingle(data.GetNumber("sz"))
		                          );
		return sca;
	}
	
	public  static Quaternion GetRot(SFSObject data){
		Quaternion rot = new Quaternion(Convert.ToSingle(data.GetNumber("rx")), 
		                                Convert.ToSingle(data.GetNumber("ry")),
		                                Convert.ToSingle(data.GetNumber("rz")),
		                                Convert.ToSingle(data.GetNumber("w"))
		                                );
		return rot;
	}

	
}
