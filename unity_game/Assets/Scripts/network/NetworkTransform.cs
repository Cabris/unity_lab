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
		Hashtable data = GetTransform(this.obj.transform);
		data.Add ("object_name", this.obj.name);
		data.Add ("cmd", "t");
		return data;
	}

	public static Hashtable GetTransform (Transform t)
	{
		Hashtable data = new Hashtable ();
		data.Add ("x", t.position.x);
		data.Add ("y", t.position.y);
		data.Add ("z", t.position.z);
		data.Add ("rx", t.rotation.x);
		data.Add ("ry", t.rotation.y);
		data.Add ("rz", t.rotation.z);
		data.Add ("w", t.rotation.w);
		return data;
	}

	public static SFSObject GetTransformAsSfs (Transform t)
	{
		SFSObject data = new SFSObject ();
		data.Put ("x", t.position.x);
		data.Put ("y", t.position.y);
		data.Put ("z", t.position.z);
		data.Put ("rx", t.rotation.x);
		data.Put ("ry", t.rotation.y);
		data.Put ("rz", t.rotation.z);
		data.Put ("w", t.rotation.w);
		return data;
	}

	// Send transform to all other users
	public void DoSend() {
		SmartFoxClient client = ClientNetworkController.GetClient();
		Hashtable h=GetData();
		client.SendXtMessage("test","b",h);
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

