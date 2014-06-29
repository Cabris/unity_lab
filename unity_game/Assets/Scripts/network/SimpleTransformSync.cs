using UnityEngine;
using System.Collections;

public class SimpleTransformSync : NetworkObject {
	
	NetworkTransform nt;
	Queue queue;
	// Use this for initialization
	void Start () {
		nt=new NetworkTransform(gameObject);
		queue=new Queue();
	}
	
	// Update is called once per frame
	void Update () {
		if(NetworkController.UserType==MyUserType.Scene&&
		   nt.UpdateIfDifferent()){
			Hashtable data=nt.GetData();
			SendMessage(data);
		}
		if(NetworkController.UserType==MyUserType.Client&&queue.Count>0){
			SmartFoxClientAPI.Data.SFSObject data=queue.Dequeue() as SmartFoxClientAPI.Data.SFSObject;
			NetworkTransformReceiver.SetTransform(transform,data);
		}
	}
	
	public override void ReceiveMessage (SmartFoxClientAPI.Data.SFSObject data)
	{
		queue.Enqueue(data);
	}
	
	
}
