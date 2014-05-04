using UnityEngine;
using System.Collections;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class PlayerAnimation : MonoBehaviour {

	protected Animator animator;
	public bool isWalking=false;
	public bool isSendmode;
	private Queue queue = new Queue();
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(animator)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			float speed=0;
			if(isWalking){
				speed=5;
			}
			else{
				speed=0;
			}
			animator.SetFloat("Speed", speed);
		}

		if(isSendmode){
			Hashtable data =new Hashtable();
			data.Add ("object_name", this.name);
			data.Add ("cmd", "a");
			data.Add ("isWalking", isWalking);
			SmartFoxClient client = ClientNetworkController.GetClient();
			client.SendXtMessage("test","b",data);
		}
		else{
			if(queue.Count>0){
				SFSObject d=queue.Dequeue() as SFSObject;
				isWalking=d.GetBool("isWalking");
			}
		}

	}

	public void ReceiveAni(SFSObject data){
		queue.Enqueue(data);
	}

}
