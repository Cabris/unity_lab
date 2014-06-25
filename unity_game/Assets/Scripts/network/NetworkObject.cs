using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;
public class NetworkObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected virtual void SendMessage(Hashtable data){
		data.Add ("object_name",name);
		data.Add ("type",GetType().ToString());
		NetworkController.SendExMsg ("test","b",data);
	}

	public virtual void ReceiveMessage(SFSObject data){

	}
}
