using UnityEngine;
using System.Collections;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class PlayerAnimation : MonoBehaviour {
	
	protected Animator animator;
	public bool isSendmode;
	public float speed=0;
	public float direction=0;
	float _speed=0, _direction=0;
	AnimatorStateInfo stateInfo;
	private Queue queue = new Queue();
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		
		if(animator)
		{
			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction);
		}

		if(isSendmode&&!isSame()){
			_speed=speed;
			_direction=direction;
			Hashtable data =new Hashtable();
			data.Add ("object_name", this.name);
			data.Add ("cmd", "a");
			data.Add ("vertical", speed);
			data.Add ("horizontal", direction);
			//SmartFoxClient client = ClientNetworkController.GetClient();
			ClientNetworkController.SendExMsg("test","b",data);

		}else if(queue.Count>0){
			SFSObject d=queue.Dequeue() as SFSObject;
			speed=(float)d.GetNumber("vertical");
			direction=(float)d.GetNumber("horizontal");
		}
	}
	
	public void ReceiveAni(SFSObject data){
		queue.Enqueue(data);
	}

	bool isSame(){
		return speed==_speed&&direction==_direction;
	}
	
}
