using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	protected Animator animator;
	bool isWalking=false;
	public Transform character;
	public float DirectionDampTime = .25f;
	
	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	public void Right(){character.Rotate(new Vector3(0,1,0)*90);}
	public void Left(){character.Rotate(new Vector3(0,1,0)*-90);}
	public void Back(){character.Rotate(new Vector3(0,1,0)*180);}

	public void StartWalk(){
		if(!isWalking)
			isWalking=true;
	}
	public void EndWork(){
		isWalking=false;
	}

	
	void Update () 
	{
		if(animator)
		{
			//get the current state
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			
			//if we're in "Run" mode, respond to input for jump, and set the Jump parameter accordingly. 
			if(stateInfo.nameHash == Animator.StringToHash("Base Layer.RunBT"))
			{
//				if(Input.GetButton("Fire1")) 
//					animator.SetBool("Jump", true );
			}
			else
			{
//				animator.SetBool("Jump", false);				
			}
			
			//float h = Input.GetAxis("Horizontal");
			//float v = Input.GetAxis("Vertical");
			float speed=0;
			if(isWalking){
				speed=5;
			}
			else{
				speed=0;
			}
//			if(Input.GetKey(KeyCode.W)){
//				speed=5;
//			}
//			else if(Input.GetKeyDown(KeyCode.A)){//left
//				character.Rotate(new Vector3(0,1,0)*-90);
//			}
//			else if(Input.GetKeyDown(KeyCode.S)){
//				character.Rotate(new Vector3(0,1,0)*180);
//			}
//			else if(Input.GetKeyDown(KeyCode.D)){//right
//				character.Rotate(new Vector3(0,1,0)*90);
//			}
//			else{
//				speed=0;
//			}
			animator.SetFloat("Speed", speed);
			//set event parameters based on user input
			//animator.SetFloat("Speed", h*h+v*v);
			//animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);
		}		
	}   		  
}
