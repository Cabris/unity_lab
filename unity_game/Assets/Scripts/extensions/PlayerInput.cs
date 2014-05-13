using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	float speed=2.5f;
	public Transform character;
	bool isTurnRight=false;
	bool isTurnLeft=false;
	PlayerAnimation animation;
	void Start () 
	{
		animation=GetComponent<PlayerAnimation>();
	}

	void Right(){
		character.Rotate(new Vector3(0,1,0)*80*Time.deltaTime, Space.World);
	}
	void Left(){
		character.Rotate(new Vector3(0,1,0)*-80*Time.deltaTime, Space.World);
	}

	public void StartWalk(){
		if(!animation.isWalking)
			animation.isWalking=true;
	}
	public void EndWalk(){
		animation.isWalking=false;
	}

	public void StartTurn(string cmd){
		if(cmd=="right"){
			isTurnRight=true;
		}
		if(cmd=="left"){
			isTurnLeft=true;
		}
	}

	public void EndTurn(string cmd){
		if(cmd=="right"){
			isTurnRight=false;
		}
		if(cmd=="left"){
			isTurnLeft=false;
		}
	}

	void Update () {
		float dt=Time.deltaTime;
		if(animation.isWalking){
			character.Translate(Vector3.forward * speed*dt);
		}
		if(isTurnLeft){
			Left();
		}
		if(isTurnRight){
			Right();
		}
	}
 		  
}
