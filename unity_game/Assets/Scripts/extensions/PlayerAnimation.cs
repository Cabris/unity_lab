using UnityEngine;
using System.Collections;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class PlayerAnimation : MonoBehaviour {
	
	protected Animator animator;
	public bool isSendmode;
	public bool left,right,up,down;
	public bool isToSend;
	AnimatorStateInfo stateInfo;
	private Queue queue = new Queue();
	void Start () {
		animator = GetComponent<Animator>();
		left=right=up=down=false;
		isToSend=false;
	}
	
	void FixedUpdate () {
		
		if(animator)
		{
			float speed=0;
			float direction=0;
			
			if(left)
				direction=-1;
			if(right)
				direction=1;
			if(up)
				speed=1;
			if(down)
				speed=-1;
			
			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction);
		}

		if(isSendmode&&isToSend){
			Hashtable data =new Hashtable();
			data.Add ("object_name", this.name);
			data.Add ("cmd", "a");
			data.Add ("left", left);
			data.Add ("right", right);
			data.Add ("up", up);
			data.Add ("down", down);
			SmartFoxClient client = ClientNetworkController.GetClient();
			client.SendXtMessage("test","b",data);
			isToSend=false;
		}else if(queue.Count>0){
			SFSObject d=queue.Dequeue() as SFSObject;
			
			left=d.GetBool("left");
			right=d.GetBool("right");
			up=d.GetBool("up");
			down=d.GetBool("down");
		}
	}
	
	public void ReceiveAni(SFSObject data){
		queue.Enqueue(data);
	}
	
}
