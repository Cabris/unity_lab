using UnityEngine;
using System.Collections;
using SmartFoxClientAPI.Data;

[RequireComponent(typeof(SphereCollider))]
public class MechinePart : NetworkObject {
	Renderer r;
	public bool isPlaced;
	[SerializeField]
	GameObject otherPart;
	// Use this for initialization
	void Start () {
		collider.isTrigger=true;
		r=GetComponentInChildren<Renderer>();
		isPlaced=false;
	}
	
	// Update is called once per frame
	void Update () {
		r.enabled=isPlaced;
		if(rigidbody!=null)
			rigidbody.isKinematic=!isPlaced;
	}

	public override void ReceiveMessage (SFSObject data)
	{
		isPlaced=data.GetBool("isPlaced");
		if(isPlaced){
			GameObject.Destroy (otherPart);
		}
	}
	
	void sendPlacedMsgToClient(){
		Hashtable data=new Hashtable();
		data.Add("isPlaced",true);
		SendMessage(data);
	}

	void OnTriggerEnter(Collider other) {
		if(NetworkController.UserType==MyUserType.Scene){
			if(other.gameObject==otherPart){
				SceneObject s=otherPart.GetComponent<SceneObject>();
				if(s!=null){
					s.ForceClientMove();
					GameObject.Destroy (otherPart);
				}
				isPlaced=true;
				sendPlacedMsgToClient();
			}
		}
	}
	
}
