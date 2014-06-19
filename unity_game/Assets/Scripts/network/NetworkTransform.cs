using UnityEngine;
using System.Collections;
//using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

// We use this class here to store and work with transform states
public class NetworkTransform {
	
	public Vector3 position;
	public Vector3 scale;
	public Quaternion rotation;
	private GameObject obj;
	
	public NetworkTransform(GameObject obj) {
		this.obj = obj;
		InitFromCurrent();
	}
	
	// Updates last state to the current transform state if the current state was changed and return true if so or false if not
	public bool UpdateIfDifferent() {
		if (obj.transform.position != this.position 
		    || obj.transform.rotation!=this.rotation
		    ||obj.transform.localScale!=this.scale) {
			InitFromCurrent();
			return true;
		}
		else {
			return false;
		}
	}

	Hashtable GetData ()
	{
		Hashtable data = GetTransformAsHash(this.obj.transform);
		data.Add ("object_name", this.obj.name);
		data.Add ("cmd", "t");
		return data;
	}

	public static Hashtable GetTransformAsHash (Transform t)
	{
		return GetTransformAsSfs(t).ToHashTable();
	}

	public static SFSObject GetTransformAsSfs (Transform t)
	{
		SFSObject data = new SFSObject ();

		string dataLine="";
		dataLine+=(t.position.x+",");
		dataLine+=(t.position.y+",");
		dataLine+=(t.position.z+",");
		dataLine+=(t.rotation.x+",");
		dataLine+=(t.rotation.y+",");
		dataLine+=(t.rotation.z+",");
		dataLine+=(t.rotation.w+",");
		dataLine+=(t.localScale.x+",");
		dataLine+=(t.localScale.y+",");
		dataLine+=(t.localScale.z);
		data.Put("dataLine",dataLine);

		return data;
	}

	// Send transform to all other users
	public void DoSend() {
		//SmartFoxClient client = NetworkController.GetClient();
		Hashtable h=GetData();
		NetworkController.SendExMsg("test","b",h);

		//SFSObject s = h.ToSFSObject ();
		//NetworkController.Send (s);

	}
	
	public void InitFromValues(Vector3 pos, Quaternion rot,Vector3 sca) {
		this.position = pos;
		this.rotation = rot;
		this.scale=sca;
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
			return (t.position == this.position 
			        && t.rotation==this.rotation
			        &&t.localScale==this.scale);
		}
		else if (n!=null) {
			return (n.position == this.position 
			        && n.rotation==this.rotation
			        &&n.scale==this.scale);
		}
		else {
			return false;
		}	        	
	}
	
	
	private void InitFromCurrent() {
		this.position = obj.transform.position;
		this.rotation = obj.transform.rotation;
		this.scale=obj.transform.localScale;
	}
	
	private void InitFromGiven(Transform trans) {
		this.position = trans.position;
		this.rotation = trans.rotation;	
		this.scale=trans.localScale;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
}

