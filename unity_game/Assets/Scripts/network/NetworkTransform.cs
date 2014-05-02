using UnityEngine;
using System.Collections;

using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

// We use this class here to store and work with transform states
public class NetworkTransform {
	
	public Vector3 position;
	public Quaternion rotation;
	private GameObject obj;
	
	public NetworkTransform(GameObject obj) {
		this.obj = obj;
		InitFromCurrent();
	}
	
	// Updates last state to the current transform state if the current state was changed and return true if so or false if not
	public bool UpdateIfDifferent() {
		if (obj.transform.position != this.position || obj.transform.rotation!=this.rotation) {
			InitFromCurrent();
			return true;
		}
		else {
			return false;
		}
	}

	Hashtable GetData ()
	{
		Hashtable data = new Hashtable ();
		//data.Put ("_cmd", "t");
		//We put _cmd = "t" here to know that this object contains transform sync data. 
		data.Add ("x", this.position.x);
		data.Add ("y", this.position.y);
		data.Add ("z", this.position.z);
		data.Add ("rx", this.rotation.x);
		data.Add ("ry", this.rotation.y);
		data.Add ("rz", this.rotation.z);
		data.Add ("w", this.rotation.w);
		data.Add ("object_name", this.obj.name);
		return data;
	}



	// Send transform to all other users
	public void DoSend() {
		SmartFoxClient client = ClientNetworkController.GetClient();
		Hashtable h=GetData();
	//	h.Add("h0","sss");
		client.SendXtMessage("test","t-b",h);
	}
	
	public void InitFromValues(Vector3 pos, Quaternion rot) {
		this.position = pos;
		this.rotation = rot;
	}
	
	// To compare with Unity transform and itself
	public override bool Equals(System.Object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		Transform t = obj as Transform;
		NetworkTransform n = obj as NetworkTransform;
		
		if (t!=null) {
			return (t.position == this.position && t.rotation==this.rotation);
		}
		else if (n!=null) {
			return (n.position == this.position && n.rotation==this.rotation);
		}
		else {
			return false;
		}	        	
	}
	
	
	private void InitFromCurrent() {
		this.position = obj.transform.position;
		this.rotation = obj.transform.rotation;	
	}
	
	private void InitFromGiven(Transform trans) {
		this.position = trans.position;
		this.rotation = trans.rotation;	
	}
	
}

