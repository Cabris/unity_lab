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
//	private NetworkTransform interpolateTo = null;  // Last state we interpolate to in receiving mode.
//	private NetworkTransform interpolateFrom;  // Point from which to start interpolation
	
	public int interpolationPoint = 0; // Current interpolation point
	public int maxInterpolationPoints = 0; // Maximum number of interpolation points;
	private float interpolationDelta = 0; // Delta value by which interpolate
	
	private FPSStorage fpsStorage;
	private Queue queue = new Queue();  // Queue to store transform states for interpolations
	
	// We call it on remote player to start receiving his transform
	public void StartReceiving() {
		lastState = new NetworkTransform(this.gameObject);	
		fpsStorage = GameObject.Find("FPS").GetComponent<FPSStorage>() as FPSStorage;
		receiveMode = true;
	}
	
	void FixedUpdate() {
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
			lastState.InitFromValues(pos, rot,sca);

			queue.Enqueue(lastState);

		}
	}
	
	// This method is called in every Fixed Update in receiving mode. And it does transform interpolation to the latest state.
	void InterpolateTransform() {

			// Take new value from queue
			if (queue.Count!=0) {
				NetworkTransform nextTransform = queue.Dequeue() as NetworkTransform;
				SetTransform(transform,nextTransform);
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
		string dataLine=data.GetString("dataLine");
		string[] datas=dataLine.Split(',');

		Vector3 pos = new Vector3(Convert.ToSingle(datas[0]), 
		                          Convert.ToSingle(datas[1]),
		                          Convert.ToSingle(datas[2])
		                          );
		return pos;
	}

	public  static Quaternion GetRot(SFSObject data){
		string dataLine=data.GetString("dataLine");
		string[] datas=dataLine.Split(',');
		Quaternion rot = new Quaternion(Convert.ToSingle(datas[3]), 
		                                Convert.ToSingle(datas[4]),
		                                Convert.ToSingle(datas[5]),
		                                Convert.ToSingle(datas[6])
		                                );
		return rot;
	}

	public  static Vector3 GetScale(SFSObject data){
		string dataLine=data.GetString("dataLine");
		string[] datas=dataLine.Split(',');
		Vector3 sca = new Vector3(Convert.ToSingle(datas[7]), 
		                          Convert.ToSingle(datas[8]),
		                          Convert.ToSingle(datas[9])
		                          );
		return sca;
	}
	


	
}
